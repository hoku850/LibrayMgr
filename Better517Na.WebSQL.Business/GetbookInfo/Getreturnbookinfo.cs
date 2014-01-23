//-----------------------------------------------------------------------
// <copyright file="Getreturnbookinfo.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: Getreturnbookinfo.cs
// * history : created by shaoyu 2014/1/21 21:20:48 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data;
using Better.Infrastructures.DBUtility;
using Better517Na.LibrayMgr.Factory;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    /// 获取(未归还)图书信息
    /// </summary>
    public class Getreturnbookinfo : DbTransaction
    {
        /// <summary>
        /// 存储借阅信息阅读器
        /// </summary>
        private IDAL.ILoanInfoAccess loanInfoAccess = null;

        /// <summary>
        /// 未归还图书信息
        /// </summary>
        private OutTimeBookInfo outtimebookInfo = new OutTimeBookInfo();

        /// <summary>
        /// 图书条形码
        /// </summary>
        private Guid bookGuid = new Guid();

        /// <summary>
        /// 查询该人借书超期数量构造器
        /// </summary>
        /// <param name="bookguid">书di</param> 
        public Getreturnbookinfo(Guid bookguid)
        {
            this.bookGuid = bookguid;
            this.loanInfoAccess = DALFactory.CreateLoanInfoAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 未归还图书信息
        /// </summary>
        public OutTimeBookInfo OuttimebookInfo
        {
            get
            {
                return this.outtimebookInfo;
            }

            set
            {
                this.outtimebookInfo = value;
            }
        }

        /// <summary>
        /// 查询该人借书超期数量
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.outtimebookInfo = this.GetReturnBookInfo(this.bookGuid, this.Connection);
        }

        /// <summary>
        /// 获取(未归还)图书信息
        /// </summary>
        /// <param name="bookguid">书di</param>
        /// <param name="conn">数据库连接</param> 
        /// <returns>该书信息</returns>
        private OutTimeBookInfo GetReturnBookInfo(Guid bookguid, IDbConnection conn)
        {
            RrturnBookinfo retbookinfo = this.loanInfoAccess.GetReturnBookInfo(false, bookguid, conn);
            OutTimeBookInfo outTimeBookInfo = new OutTimeBookInfo();
            if (!string.IsNullOrEmpty(retbookinfo.UserName))
            {
                outTimeBookInfo.UserName = retbookinfo.UserName;
                outTimeBookInfo.BookOrderNum = retbookinfo.BookOrderNum;
                outTimeBookInfo.Booktitle = retbookinfo.Booktitle;
                outTimeBookInfo.BorrowDate = retbookinfo.BorrowDate;
            }

            return outTimeBookInfo;
        }
    }
}
