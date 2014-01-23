//-----------------------------------------------------------------------
// <copyright file="ILoanInfoAccess.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: ILoanInfoAccess.cs
// * history : created by shaoyu 2014/1/17 11:00:37 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.IDAL
{
    /// <summary>
    /// 借阅信息查询接口
    /// </summary>
    public interface ILoanInfoAccess
    {
        /// <summary>
        /// 查询该人借阅数量
        /// </summary>
        /// <param name="name">借阅人姓名</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>借阅数量</returns>
        int GetbooknumbyUsername(string name, IDbConnection conn);

        /// <summary>
        /// 查询该人借书超期数量
        /// </summary>
        /// <param name="name">借阅人姓名</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>超期信息</returns>
        UerInfo GetOutTimebooksum(string name, IDbConnection conn);

        /// <summary>
        /// 插入借书信息
        /// </summary>
        /// <param name="orderNum">图书条形码</param>
        /// <param name="userName">借阅人姓名</param>
        /// <param name="trans">事务</param>
        /// <returns>影响数据库行数</returns>
        int BorrowBook(Guid orderNum, string userName, IDbTransaction trans);

        /// <summary>
        /// 还书。只将还书日期添上（不缴纳罚款）
        /// </summary>
        /// <param name="bookguid">图书条形码</param>
        /// <param name="trans">事务</param>
        /// <returns>返回影响的行数</returns>
        int ReturnBook(Guid bookguid, IDbTransaction trans);

        /// <summary>
        /// 获取(归还/未归还)图书信息
        /// </summary>
        /// <param name="isreturn">是否归还</param>
        /// <param name="bookguid">书di</param>
        /// <param name="conn">连接</param>
        /// <returns>该书信息</returns>
        RrturnBookinfo GetReturnBookInfo(bool isreturn, Guid bookguid, IDbConnection conn);

        /// <summary>
        /// 写入罚款
        /// </summary>
        /// <param name="bookguid">借书ID</param>
        /// <param name="money">罚款金额</param>
        /// <param name="trans">事务</param>
        /// <returns>影响的行数</returns>
        int WritePaied(Guid bookguid, decimal money, IDbTransaction trans);

        /// <summary>
        /// 缴纳罚款置1
        /// </summary>
        /// <param name="bookguid">超期图书条形码</param>
        /// <param name="trans">事务</param>
        /// <returns>影响的行数</returns>
        int PayMoneyByguid(Guid bookguid, IDbTransaction trans);

        /// <summary>
        /// 查询借书人 已还图书中还未付清罚款的信息
        /// </summary>
        /// <param name="name">借书人</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>欠款信息</returns>
        List<RrturnBookinfo> GetReturnUserInfo(string name, IDbConnection conn);

        /// <summary>
        /// 付清罚款
        /// </summary>
        /// <param name="name">借书人</param>
        /// <param name="trans">事务</param>
        /// <returns>返回影响的行数</returns>
        int PayMoney(string name, IDbTransaction trans);
      
        /// <summary>
        /// 获取借阅信息
        /// </summary>
        /// <param name="isback">是否已还</param>
        /// <param name="username">用户id</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>借阅信息表</returns>
        List<RrturnBookinfo> GetBorrowBookInfo(bool isback, string username, IDbConnection conn);
    }
}
