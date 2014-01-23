//-----------------------------------------------------------------------
// <copyright file="GetBookInfoBybookid.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetBookInfoBybookid.cs
// * history : created by shaoyu 2014/1/21 17:25:41 
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
    /// 通过ID获取图书信息
    /// </summary>
    public class Getbookinfobybookid : DbTransaction
    {
        /// <summary>
        /// 图书信息查询器
        /// </summary>
        private IDAL.IBookInfoAccess bookInfoAccess = null;

        /// <summary>
        /// 返回的图书信息
        /// </summary>
        private Bookinfo bookinfo = new Bookinfo();

        /// <summary>
        /// 图书guid
        /// </summary>
        private Guid bookguid = new Guid();

        /// <summary>
        /// 通过ID获取图书信息
        /// </summary>
        /// <param name="bookID">图书ID</param>
        public Getbookinfobybookid(Guid bookID)
        {
            this.bookguid = bookID;
            this.bookInfoAccess = DALFactory.CreateBookInfoAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 返回的图书信息
        /// </summary>
        public Bookinfo Bookinfo
        {
            get
            {
                return this.bookinfo;
            }

            set
            {
                this.bookinfo = value;
            }
        }

        /// <summary>
        /// 获取图书信息
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.bookinfo = this.GetBookInfoBybookid(this.bookguid, this.Connection);
        }

        /// <summary>
        /// 根据图书ID查询图书信息
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>图书信息</returns>
        private Bookinfo GetBookInfoBybookid(Guid bookID, IDbConnection conn)
        {
            return this.bookInfoAccess.GetBookInfoBybookid(bookID, conn);
        }
    }
}
