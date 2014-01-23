//-----------------------------------------------------------------------
// <copyright file="QueryBorrowBookMgr.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: QueryBorrowBookMgr.cs
// * history : created by shaoyu 2014/1/20 16:26:02 
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Data;
using Better.Infrastructures.DBUtility;
using Better517Na.LibrayMgr.Factory;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    /// 查询借阅类
    /// </summary>
    public class QueryBorrowBookMgr : DbTransaction
    {
        /// <summary>
        /// 存储借阅信息阅读器
        /// </summary>
        private IDAL.ILoanInfoAccess loanInfoAccess = null;

        /// <summary>
        /// 是否已还
        /// </summary>
        private bool isBack = false;

        /// <summary>
        /// 用户id
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// 还书信息
        /// </summary>
        private List<RrturnBookinfo> rturnBookinfoList = new List<RrturnBookinfo>();

        /// <summary>
        /// 获取BookItem构造器
        /// </summary>
        /// <param name="isback">图书条形码</param>
        /// <param name="username">用户</param>
        public QueryBorrowBookMgr(bool isback, string username)
        {
            this.isBack = isback;
            this.userName = username;
            this.loanInfoAccess = DALFactory.CreateLoanInfoAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 获取还书信息
        /// </summary>
        public List<RrturnBookinfo> RturnBookinfoList
        {
            get 
            {
                return this.rturnBookinfoList;
            }

            set 
            { 
                this.rturnBookinfoList = value;
            }
        }

        /// <summary>
        /// 获取图书信息
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.rturnBookinfoList = this.GetBorrowBookInfo(this.isBack, this.userName, this.Connection);
        }

        /// <summary>
        /// 获取借阅信息
        /// </summary>
        /// <param name="isback">是否已还</param>
        /// <param name="username">用户id</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>借阅信息表</returns>
        private List<RrturnBookinfo> GetBorrowBookInfo(bool isback, string username, IDbConnection conn)
        {
            return this.loanInfoAccess.GetBorrowBookInfo(isback, username, conn);
        }
    }
}
