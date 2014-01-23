//-----------------------------------------------------------------------
// <copyright file="CheckUserInfoMgr.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: CheckUserInfoMgr.cs
// * history : created by shaoyu 2014/1/21 17:11:00 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Better.Infrastructures.DBUtility;
using Better.Infrastructures.Log;
using Better517Na.LibrayMgr.Factory;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    /// CheckUserInfoMgr
    /// </summary>
    public class CheckUserInfoMgr : DbTransaction
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        private string resturnmessage = string.Empty;

        /// <summary>
        /// 借阅人
        /// </summary>
        private string username = string.Empty;

        /// <summary>
        /// 借阅信息查阅器
        /// </summary>
        private IDAL.ILoanInfoAccess loanInfoAccess = null;

        /// <summary>
        /// 图书信息查阅器
        /// </summary>
        private IDAL.IBookItemAccess bookItemAccess = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">借阅人</param>
        public CheckUserInfoMgr(string name)
        {
            this.username = name;
            this.loanInfoAccess = DALFactory.CreateLoanInfoAccess();
            this.bookItemAccess = DALFactory.CreateBookItemAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        public string Resturnmessage
        {
            get
            {
                return this.resturnmessage;
            }

            set
            {
                this.resturnmessage = value;
            }
        }

        /// <summary>
        /// 检测借书人信息
        /// </summary>
        /// <param name="username">借书人</param>
        /// <returns>提示信息</returns>
        public string Checkuser(string username)
        {
            string message = string.Empty;
            UerInfo userinfo = new UerInfo();
            GetLoanInfoMgr getLoanInfoMgr = new GetLoanInfoMgr();
            userinfo = getLoanInfoMgr.CheckUserInfo(username);
            if (userinfo.Booknum >= 5)
            {
                userinfo.Isborrow = false;
                message = "每人最多可以借阅5本书，你已经借阅5本书，不能再借阅。#0";
            }
            else if (userinfo.OutBack > 0)
            {
                GetBookInfoMgr getBookInfoMgr = new GetBookInfoMgr();
                GetBookItemInfoMgr getBookItemInfoMgr = new GetBookItemInfoMgr();
                StringBuilder messagesb = new StringBuilder("您所借图书：");
                foreach (Guid orderid in userinfo)
                {
                    Guid bookid = getBookItemInfoMgr.GetBookID(orderid);
                    Bookinfo borrowbookinfo = getBookInfoMgr.GetBookInfoBybookid(bookid);
                    messagesb.Append("《" + borrowbookinfo.Title + "》");
                }

                messagesb.Append(" 超期依然未归还，请归还再借阅。#0");
                message = messagesb.ToString();
                userinfo.Isborrow = false;
            }
            else
            {
                message = this.GetreturnMessage(username);
            }

            return message;
        }

        /// <summary>
        /// 插入借书信息
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.resturnmessage = this.Checkuser(this.username);
        }

        /// <summary>
        /// 组装罚款提示信息
        /// </summary>
        /// <param name="username">借阅人</param>
        /// <returns>提示信息</returns>
        private string GetreturnMessage(string username)
        {
            string message = string.Empty;
            GetLoanInfoMgr getLoanInfoMgr = new GetLoanInfoMgr();
            List<RrturnBookinfo> returnbooklist = getLoanInfoMgr.GetReturnInfo(username);
            if (returnbooklist.Count > 0)
            {
                decimal payforbook = 0;
                StringBuilder messagesb = new StringBuilder(" 你已还图书");
                foreach (RrturnBookinfo tempRrturnBookinfo in returnbooklist)
                {
                    // 你已还图书’xxx’、’xxx’未缴纳罚款，罚款金额为 25.00 元，请先缴纳罚款再借阅
                    messagesb.Append("《" + tempRrturnBookinfo.Booktitle + "》");
                    payforbook += tempRrturnBookinfo.PayMoney;
                }

                messagesb.Append("未缴纳罚款，罚款金额为:");
                messagesb.Append(payforbook.ToString());
                messagesb.Append("元，请先缴纳罚款再借阅。#1");
                message = messagesb.ToString();
            }

            return message;
        }
    }
}
