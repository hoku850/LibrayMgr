//-----------------------------------------------------------------------
// <copyright file="GetLoanInfoMgr.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetLoanInfoMgr.cs
// * history : created by shaoyu 2014/1/18 8:46:27 
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
    /// GetLoanInfoMgr
    /// </summary>
    public class GetLoanInfoMgr
    {
        /// <summary>
        /// 检测借书人借书信息
        /// </summary>
        /// <param name="userName">借书人</param>
        /// <returns>借书人信息</returns>
        public UerInfo CheckUserInfo(string userName)
        {
            UerInfo userinfo = new UerInfo();

            int booknum = 0;
            string result = string.Empty;

            booknum = this.GetbooknumbyUsername(userName);
            userinfo.Booknum = booknum;
            if (booknum < 5)
            {
                userinfo = this.GetOutTimebooksum(userName);
                userinfo.OutBack = userinfo.Count;
            }

            return userinfo;
        }

        /// <summary>
        /// 查询借书人 已还图书中还未付清罚款的信息
        /// </summary>
        /// <param name="name">借书人</param>
        /// <returns>欠款信息</returns>
        public List<RrturnBookinfo> GetReturnInfo(string name)
        {
            GetReturnInfo getReturnInfo = new GetReturnInfo(name);
            getReturnInfo.Execute();
            return getReturnInfo.RturnBookinfoList;
        }

        /// <summary>
        /// 获取(未归还)图书信息
        /// </summary>
        /// <param name="bookguid">书di</param>
        /// <returns>该书信息</returns>
        public OutTimeBookInfo GetReturnBookInfo(Guid bookguid)
        {
            Getreturnbookinfo getreturnbookinfo = new Getreturnbookinfo(bookguid);
            getreturnbookinfo.Execute();
            return getreturnbookinfo.OuttimebookInfo;
        }

        /// <summary>
        /// 查询该人借阅数量
        /// </summary>
        /// <param name="name">借阅人姓名</param>
        /// <returns>借阅数量</returns>
        private int GetbooknumbyUsername(string name)
        {
            GetBookNumByUserName getBookNumByUserName = new GetBookNumByUserName(name);
            getBookNumByUserName.Execute();
            return getBookNumByUserName.Borrowbokknum;
        }

        /// <summary>
        /// 查询该人借书超期数量
        /// </summary>
        /// <param name="name">借阅人姓名</param>
        /// <returns>超期数量</returns>
        private UerInfo GetOutTimebooksum(string name)
        {
            GetOutTimeBookSum getOutTimeBookSum = new GetOutTimeBookSum(name);
            getOutTimeBookSum.Execute();
            return getOutTimeBookSum.UerInfo;
        }
    }
}
