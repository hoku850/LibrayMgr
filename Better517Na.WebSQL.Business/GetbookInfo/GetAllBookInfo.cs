//-----------------------------------------------------------------------
// <copyright file="GetAllBookInfo.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetAllBookInfo.cs
// * history : created by shaoyu 2014/1/21 17:34:29 
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
    /// GetAllBookInfo
    /// </summary>
    public class Getallbookinfo : DbTransaction
    {
        /// <summary>
        /// 图书信息查询器
        /// </summary>
        private IDAL.IBookInfoAccess bookInfoAccess = null;

        /// <summary>
        /// 返回的图书信息
        /// </summary>
        private List<Bookinfo> bookinfoList = new List<Bookinfo>();

        /// <summary>
        /// 通过ID获取图书信息
        /// </summary>
        public Getallbookinfo()
        {
            this.bookInfoAccess = DALFactory.CreateBookInfoAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 返回的图书信息
        /// </summary>
        public List<Bookinfo> BookinfoList
        {
            get
            {
                return this.bookinfoList;
            }

            set
            {
                this.bookinfoList = value;
            }
        }

        /// <summary>
        /// 获取图书信息
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.bookinfoList = this.GetAllBookInfo(this.Connection);
        }

        /// <summary>
        /// 获取全部图书信息
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <returns>图书信息</returns>
        private List<Bookinfo> GetAllBookInfo(IDbConnection conn)
        {
            return this.bookInfoAccess.GetAllBookInfo(conn);
        }
    }
}
