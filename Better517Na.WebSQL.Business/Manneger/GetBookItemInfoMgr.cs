//-----------------------------------------------------------------------
// <copyright file="GetBookItemInfoMgr.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetBookItemInfoMgr.cs
// * history : created by shaoyu 2014/1/17 14:13:08 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using Better517Na.LibrayMgr.Factory;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    /// 获取图书信息类
    /// </summary>
    public class GetBookItemInfoMgr
    {
        /// <summary>
        /// 查询总的图书藏书与可借书信息
        /// </summary>
        /// <returns>总的图书藏书与可借书信息</returns>
        public List<DisplayInfo> QuerySumISLoan()
        {
            QuerysumISLoan querysumISLoan = new QuerysumISLoan();
            querysumISLoan.Execute();
            return querysumISLoan.DisplayInfoList;
        }

        /// <summary>
        /// 根据图书ID查询图书总数
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <returns>该书总数</returns>
        public int GetKindBookSum(Guid bookID)
        {
            GetkindbookSum getkindbookSum = new GetkindbookSum(bookID);
            getkindbookSum.Execute();
            return getkindbookSum.Booksum;
        }

        /// <summary>
        /// 根据图书ID查询图书可借数
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <returns>可借数</returns>
        public int GetISloanBookSum(Guid bookID)
        {
            GetisloanbookSum getisloanbookSum = new GetisloanbookSum(bookID);
            getisloanbookSum.Execute();
            return getisloanbookSum.Booksum;
        }

        /// <summary>
        /// 根据图书条形码查ID
        /// </summary>
        /// <param name="ordernum">图书条形码</param>
        /// <returns>图书ID</returns>
        public Guid GetBookID(Guid ordernum)
        {
            GetTheBookID getbookID = new GetTheBookID(ordernum);
            getbookID.Execute();
            return getbookID.BookID;
        }
    }
}
