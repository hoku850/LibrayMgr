//-----------------------------------------------------------------------
// <copyright file="QuerysumISLoan.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: QuerysumISLoan.cs
// * history : created by shaoyu 2014/1/21 21:39:40 
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
    /// 查询总的图书藏书与可借书信息
    /// </summary>
    public class QuerysumISLoan : DbTransaction
    {
        /// <summary>
        /// 图书信息查阅器
        /// </summary>
        private IDAL.IBookItemAccess bookItemAccess = null;

        /// <summary>
        /// 总的图书藏书与可借书信息
        /// </summary>
        private List<DisplayInfo> displayInfoList = new List<DisplayInfo>();

        /// <summary>
        ///  查询总的图书藏书与可借书信息构造器
        /// </summary>
        public QuerysumISLoan()
        {
            this.bookItemAccess = DALFactory.CreateBookItemAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 总的图书藏书与可借书信息
        /// </summary>
        public List<DisplayInfo> DisplayInfoList
        {
            get { return this.displayInfoList; }

            set { this.displayInfoList = value; }
        }

        /// <summary>
        /// 获取总的图书藏书与可借书信息
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.displayInfoList = this.QuerySumISLoan(this.Connection);
        }

        /// <summary>
        /// 查询总的图书藏书与可借书信息
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <returns>总的图书藏书与可借书信息</returns>
        private List<DisplayInfo> QuerySumISLoan(IDbConnection conn)
        {
            return this.bookItemAccess.QuerySumISLoan(conn);
        }
    }
}
