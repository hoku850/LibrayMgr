//-----------------------------------------------------------------------
// <copyright file="IBookInfoAccess.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: IBookInfoAccess.cs
// * history : created by shaoyu 2014/1/16 9:19:16 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.IDAL
{
    /// <summary>
    /// 员工数据访问接口类
    /// </summary>
    public interface IBookInfoAccess
    {
        /// <summary>
        /// 根据图书ID查询图书信息
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>图书信息</returns>
        Bookinfo GetBookInfoBybookid(Guid bookID, IDbConnection conn);

        /// <summary>
        /// 获取全部图书信息
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <returns>图书信息</returns>
        List<Bookinfo> GetAllBookInfo(IDbConnection conn);

        /// <summary>
        /// 模糊查询图书数
        /// </summary>
        /// <param name="bookname">查询关键字</param>
        /// <param name="conn">IDbConnection（数据库连接对象）</param>
        /// <returns>获得满足筛选条件的图书信息</returns>
        List<Bookinfo> QueryBookByName(string bookname, IDbConnection conn);
       
         /// <summary>
        /// 根据图书条形码查询借阅信息
        /// </summary>
        /// <param name="bookordernum">图书条形码</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>还书信息</returns>
        RrturnBookinfo QueryReturnBook(Guid bookordernum, IDbConnection conn);

        /// <summary>
        /// 借阅查询
        /// </summary>
        /// <param name="username">借阅人</param>
        /// <param name="isdisreturn">是否查询已还图书</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>借阅信息</returns>
        List<RrturnBookinfo> QueryBorrowBookInfo(string username, bool isdisreturn, IDbConnection conn);
    }
}
