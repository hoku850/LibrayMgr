//-----------------------------------------------------------------------
// <copyright file="PayMoneyAndBorrowMgr.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: PayMoneyAndBorrowMgr.cs
// * history : created by shaoyu 2014/1/19 22:06:49 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using Better.Infrastructures.DBUtility;
using Better.Infrastructures.Log;
using Better517Na.LibrayMgr.Factory;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    /// 付款借书类
    /// </summary>
    public class PayMoneyAndBorrowMgr : DbTransaction
    {
        /// <summary>
        /// 多线程数据同步锁，解决资源竞态问题
        /// </summary>
        private static object objHelp = new object();

        /// <summary>
        /// 借阅人
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// 借阅图书条形码
        /// </summary>
        private Guid bookguid = new Guid();

        /// <summary>
        /// 借阅信息查阅器
        /// </summary>
        private IDAL.ILoanInfoAccess loanInfoAccess = null;

        /// <summary>
        /// 图书信息查阅器
        /// </summary>
        private IDAL.IBookItemAccess bookItemAccess = null;

        /// <summary>
        /// 是否借阅成功
        /// </summary>
        private bool issuccess = false;

        /// <summary>
        /// 付款并借书
        /// </summary>
        /// <param name="name">借书人</param>
        /// <param name="booguid">书本条形码</param>
        public PayMoneyAndBorrowMgr(string name, Guid booguid)
        {
            this.userName = name;
            this.bookguid = booguid;
            this.loanInfoAccess = DALFactory.CreateLoanInfoAccess();
            this.bookItemAccess = DALFactory.CreateBookItemAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = true;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 付款是否成功
        /// </summary>
        public bool Issuccess
        {
            get { return this.issuccess; }

            set { this.issuccess = value; }
        }

        /// <summary>
        /// 付款
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.Issuccess = this.PayMoneyandBorrowBook(this.userName, this.bookguid);
        }

        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="name">付款姓名</param>
        /// <param name="orderNum">欠款图书条形码</param>
        /// <returns>是否成功</returns>
        private bool PayMoneyandBorrowBook(string name, Guid orderNum)
        {
            bool resule = false;
            if (this.loanInfoAccess.PayMoney(name, this.Transaction) != -1)
            {
                // 往图书信息表里面写
                if (this.bookItemAccess.BorrowBook(orderNum, this.Transaction))
                {
                    // 往借阅信息表里面写
                    if (1 == this.loanInfoAccess.BorrowBook(orderNum, name, this.Transaction))
                    {
                        resule = true;
                    }
                    else
                    {
                        AppException appexp = new AppException("付款，借阅失败", ExceptionLevel.Info);
                        resule = false;
                        throw appexp;
                    }
                }
                else
                {
                    AppException appexp = new AppException("图书已借出，付款不成功！", ExceptionLevel.Info);
                    resule = false;
                    throw appexp;
                }
            }
            else
            {
                AppException appexp = new AppException("付款不成功！", ExceptionLevel.Info);
                resule = false;
                throw appexp;
            }

            return resule;
        }
    }
}
