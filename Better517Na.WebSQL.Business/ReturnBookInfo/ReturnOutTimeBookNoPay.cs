//-----------------------------------------------------------------------
// <copyright file="ReturnOutTimeBookNoPay.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: ReturnOutTimeBookNoPay.cs
// * history : created by shaoyu 2014/1/20 10:44:05 
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
    /// 归还超期图书暂不缴纳罚款
    /// </summary>
    public class ReturnOutTimeBookNoPay : DbTransaction
    {
        /// <summary>
        /// 多线程数据同步锁，解决资源竞态问题
        /// </summary>
        private static object objHelp = new object();

        /// <summary>
        /// 借阅图书条形码
        /// </summary>
        private Guid bookguid = new Guid();

        /// <summary>
        /// 图书超期罚款
        /// </summary>
        private decimal bookfine = 0;

        /// <summary>
        /// 借阅信息查阅器
        /// </summary>
        private IDAL.ILoanInfoAccess loanInfoAccess = null;

        /// <summary>
        /// 图书信息查阅器
        /// </summary>
        private IDAL.IBookItemAccess bookItemAccess = null;

        /// <summary>
        /// 是否还书成功
        /// </summary>
        private bool issuccess = false;

        /// <summary>
        /// 还书
        /// </summary>
        /// <param name="booguid">图书条形码</param>
        /// <param name="fine">欠款</param>
        public ReturnOutTimeBookNoPay(Guid booguid, decimal fine)
        {
            this.bookguid = booguid;
            this.bookfine = fine;
            this.loanInfoAccess = DALFactory.CreateLoanInfoAccess();
            this.bookItemAccess = DALFactory.CreateBookItemAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = true;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 还书是否成功
        /// </summary>
        public bool Issuccess
        {
            get { return this.issuccess; }

            set { this.issuccess = value; }
        }

        /// <summary>
        /// 归还超期图书暂不缴纳罚款
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.Issuccess = this.ReturnBookNoPay(this.bookguid, this.bookfine);
        }

        /// <summary>
        /// 归还超期图书暂不缴纳罚款
        /// </summary>
        /// <param name="orderNum">条形码</param>
        /// <param name="fine">罚款</param>
        /// <returns>是否成功</returns>
        private bool ReturnBookNoPay(Guid orderNum, decimal fine)
        {
            bool issuccess = false;

            // 写入罚款
            if (this.loanInfoAccess.WritePaied(orderNum, fine, this.Transaction) == 1)
            {
                // 写入还书日期
                if (this.loanInfoAccess.ReturnBook(orderNum, this.Transaction) == 1)
                {
                    // 图书入库
                    if (this.bookItemAccess.ReturnBook(orderNum, this.Transaction))
                    {
                        issuccess = true;
                    }
                    else
                    {
                        AppException appexp = new AppException("图书入库失败，还书失败", ExceptionLevel.Info);
                        issuccess = false;
                        throw appexp;
                    }
                }
                else
                {
                    AppException appexp = new AppException("还书失败", ExceptionLevel.Info);
                    issuccess = false;
                    throw appexp;
                }
            }
            else
            {
                AppException appexp = new AppException("还书失败", ExceptionLevel.Info);
                issuccess = false;
                throw appexp;
            }

            return issuccess;
        }
    }
}
