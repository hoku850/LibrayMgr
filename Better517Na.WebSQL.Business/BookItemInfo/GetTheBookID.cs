//-----------------------------------------------------------------------
// <copyright file="GetTheBookID.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetTheBookID.cs
// * history : created by shaoyu 2014/1/21 21:57:12 
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
    /// 根据图书条形码查ID
    /// </summary>
    public class GetTheBookID : DbTransaction
    {
        /// <summary>
        /// 图书信息查阅器
        /// </summary>
        private IDAL.IBookItemAccess bookItemAccess = null;

        /// <summary>
        /// 图书条形码
        /// </summary>
        private Guid ordernum = new Guid();

        /// <summary>
        /// 图书ID
        /// </summary>
        private Guid bookID = new Guid();

        /// <summary>
        ///  根据图书条形码查ID
        /// </summary>
        /// <param name="bookGuid">图书条形码</param>
        public GetTheBookID(Guid bookGuid)
        {
            this.ordernum = bookGuid;
            this.bookItemAccess = DALFactory.CreateBookItemAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 该类图书可借数
        /// </summary>
        public Guid BookID
        {
            get
            {
                return this.bookID;
            }

            set
            {
                this.bookID = value;
            }
        }

        /// <summary>
        /// 根据图书ID查询该类图书可借数
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.bookID = this.GettheBookID(this.ordernum, this.Connection);
        }

        /// <summary>
        /// 根据图书条形码查ID
        /// </summary>
        /// <param name="ordernum">图书条形码</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>图书ID</returns>
        private Guid GettheBookID(Guid ordernum, IDbConnection conn)
        {
            Guid sum = this.bookItemAccess.GetBookID(ordernum, conn);
            return sum;
        }
    }
}
