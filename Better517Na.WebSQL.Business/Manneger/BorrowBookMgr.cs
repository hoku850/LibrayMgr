//-----------------------------------------------------------------------
// <copyright file="BorrowBookMgr.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: BorrowBookMgr.cs
// * history : created by shaoyu 2014/1/17 17:27:32 
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
    /// 借书类
    /// </summary>
    public class BorrowBookMgr : DbTransaction
    {
        /// <summary>
        /// 借阅人
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// 条形码
        /// </summary>
        private Guid ordernum = new Guid();

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
        /// 借书管理器
        /// </summary>
        /// <param name="name">借书人</param>
        /// <param name="orderNum">图书条形码</param>
        public BorrowBookMgr(string name, Guid orderNum)
        {
            this.ordernum = orderNum;
            this.userName = name;
            this.loanInfoAccess = DALFactory.CreateLoanInfoAccess();
            this.bookItemAccess = DALFactory.CreateBookItemAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = true;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 是否借阅成功
        /// </summary>
        public bool Issuccess
        {
            get
            {
                return this.issuccess;
            }

            set
            {
                this.issuccess = value;
            }
        }

        /// <summary>
        /// 插入借书信息
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.Issuccess = this.BorrowBook(this.userName, this.ordernum);
        }

        /// <summary>
        /// 借阅图书
        /// </summary>
        /// <param name="name">借阅者姓名</param>
        /// <param name="orderNum">图书条形码</param>
        /// <returns>是否借阅成功</returns>
        private bool BorrowBook(string name, Guid orderNum)
        {
            bool resule = false;

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
                    AppException appexp = new AppException("写入失败", ExceptionLevel.Info);
                    resule = false;
                    throw appexp;
                }
            }
            else
            {
                AppException appexp = new AppException("已经借出", ExceptionLevel.Info);
                resule = false;
                throw appexp;
            }

            return resule;
        }
    }
}
