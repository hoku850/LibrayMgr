//-----------------------------------------------------------------------
// <copyright file="GetBookInfoMgr.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetBookInfoMgr.cs
// * history : created by shaoyu 2014/1/17 14:38:50 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    /// 获取图书信息类
    /// </summary>
    public class GetBookInfoMgr
    {
        /// <summary>
        /// 根据图书ID查询图书信息
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <returns>图书信息</returns>
        public Bookinfo GetBookInfoBybookid(Guid bookID)
        {
            Getbookinfobybookid getBookInfoBybookid = new Getbookinfobybookid(bookID);
            getBookInfoBybookid.Execute();
            return getBookInfoBybookid.Bookinfo;
        }

        /// <summary>
        /// 模糊查询图书数
        /// </summary>
        /// <param name="bookname">查询关键字</param>
        /// <returns>获得满足筛选条件的图书信息</returns>
        public List<Bookinfo> QueryBookByName(string bookname)
        {
            Querybookbyname queryBookByName = new Querybookbyname(bookname);
            queryBookByName.Execute();
            return queryBookByName.BookinfoList;
        }

        /// <summary>
        /// 获取全部图书信息
        /// </summary>
        /// <returns>图书信息</returns>
        public List<Bookinfo> GetAllBookInfo()
        {
            Getallbookinfo getAllBookInfo = new Getallbookinfo();
            getAllBookInfo.Execute();
            return getAllBookInfo.BookinfoList;
        }
    }
}
