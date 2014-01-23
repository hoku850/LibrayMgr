//-----------------------------------------------------------------------
// <copyright file="GetkindbookSum.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetkindbookSum.cs
// * history : created by shaoyu 2014/1/21 21:45:35 
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
    /// 查询一类图书总数
    /// </summary>
    public class GetkindbookSum : DbTransaction
    {
         /// <summary>
        /// 图书信息查阅器
        /// </summary>
        private IDAL.IBookItemAccess bookItemAccess = null;

        /// <summary>
        /// 该书总数
        /// </summary>
        private int booksum = 0;

        /// <summary>
        /// 图书ID
        /// </summary>
        private Guid bookid = new Guid();

        /// <summary>
        ///  查询该类图书总数
        /// </summary>
        /// <param name="bookID">图书ID</param>
        public GetkindbookSum(Guid bookID)
        {
            this.bookid = bookID;
            this.bookItemAccess = DALFactory.CreateBookItemAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 该类图书总数
        /// </summary>
        public int Booksum
        {
            get 
            { 
                return this.booksum;
            }

            set 
            { 
                this.booksum = value;
            }
        }

        /// <summary>
        /// 根据图书ID查询图书总数
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.booksum = this.GetKindBookSum(this.bookid, this.Connection);
        }

        /// <summary>
        /// 根据图书ID查询图书总数
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>该书总数</returns>
        private int GetKindBookSum(Guid bookID, IDbConnection conn)
        {
            int sum = 0;
            sum = this.bookItemAccess.GetKindBookSum(bookID, conn);
            return sum;
        }
    }
}
