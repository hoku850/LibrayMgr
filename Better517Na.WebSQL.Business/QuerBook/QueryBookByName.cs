//-----------------------------------------------------------------------
// <copyright file="QueryBookByName.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: QueryBookByName.cs
// * history : created by shaoyu 2014/1/21 17:38:43 
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
    /// 模糊查询
    /// </summary>
    public class Querybookbyname : DbTransaction
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
        /// 图书名称
        /// </summary>
        private string bookName = string.Empty;

        /// <summary>
        /// 通过ID获取图书信息
        /// </summary>
        /// <param name="booknaem">查询关键字</param>
        public Querybookbyname(string booknaem)
        {
            this.bookName = booknaem;
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
            this.bookinfoList = this.QueryBookByName(this.bookName, this.Connection);
        }

        /// <summary>
        /// 模糊查询图书数
        /// </summary>
        /// <param name="bookname">查询关键字</param>
        /// <param name="conn">IDbConnection（数据库连接对象）</param>
        /// <returns>获得满足筛选条件的图书信息</returns>
        private List<Bookinfo> QueryBookByName(string bookname, IDbConnection conn)
        {
            return this.bookInfoAccess.QueryBookByName(bookname, conn);
        }
    }
}
