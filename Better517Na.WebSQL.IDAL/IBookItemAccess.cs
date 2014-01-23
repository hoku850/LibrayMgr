//-----------------------------------------------------------------------
// <copyright file="IBookItemAccess.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: IBookItemAccess.cs
// * history : created by shaoyu 2014/1/16 9:18:52 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.IDAL
{
    /// <summary>
    /// 部门数据访问接口类 
    /// </summary>
    public interface IBookItemAccess
    {
        /// <summary>
        /// 查询总的图书藏书与可借书信息
        /// </summary>
        /// <param name="conn">IDbConnection数据库连接对象</param> 
        /// <returns>总的图书藏书与可借书信息</returns>
        List<DisplayInfo> QuerySumISLoan(IDbConnection conn);

        /// <summary>
        /// 图书借阅
        /// </summary>
        /// <param name="ordernum">图书条形码</param>
        /// <param name="trans">事务</param>
        /// <returns>成功返回true 失败false</returns>
        bool BorrowBook(Guid ordernum, IDbTransaction trans);

        /// <summary>
        /// 还书,图书入库
        /// </summary>
        /// <param name="ordernum">图书条形码</param>
        /// <param name="trans">事务</param>
        /// <returns>返回影响的行数</returns>
        bool ReturnBook(Guid ordernum, IDbTransaction trans);

        /// <summary>
        /// 根据图书ID查询图书总数
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>该书总数</returns>
        int GetKindBookSum(Guid bookID, IDbConnection conn);

        /// <summary>
        /// 根据图书ID查询图书可借数
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>可借数</returns>
        int GetISloanBookSum(Guid bookID, IDbConnection conn);

        /// <summary>
        /// 根据图书条形码查ID
        /// </summary>
        /// <param name="ordernum">图书条形码</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>图书ID</returns>
        Guid GetBookID(Guid ordernum, IDbConnection conn);
    }
}
