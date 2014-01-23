//-----------------------------------------------------------------------
// <copyright file="GetReturnInfo.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetReturnInfo.cs
// * history : created by shaoyu 2014/1/21 21:03:54 
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
    /// GetReturnInfo
    /// </summary>
    public class GetReturnInfo : DbTransaction
    {
        /// <summary>
        /// 存储借阅信息阅读器
        /// </summary>
        private IDAL.ILoanInfoAccess loanInfoAccess = null;

        /// <summary>
        /// 已还图书未付款的图书信息
        /// </summary>
        private List<RrturnBookinfo> rturnBookinfoList = new List<RrturnBookinfo>();

        /// <summary>
        /// 借阅人姓名
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// 通过ID获取图书信息
        /// </summary>
        /// <param name="usernaem">借阅人姓名</param>
        public GetReturnInfo(string usernaem)
        {
            this.userName = usernaem;
            this.loanInfoAccess = DALFactory.CreateLoanInfoAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        ///  已还图书未付款的图书信息
        /// </summary>
        public List<RrturnBookinfo> RturnBookinfoList
        {
            get
            {
                return this.rturnBookinfoList;
            }

            set
            {
                this.rturnBookinfoList = value;
            }
        }

        /// <summary>
        /// 获取图书信息
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.rturnBookinfoList = this.GetreturnInfo(this.userName, this.Connection);
        }

        /// <summary>
        /// 查询借书人 已还图书中还未付清罚款的信息
        /// </summary>
        /// <param name="name">借阅人姓名</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>欠款信息</returns>
        private List<RrturnBookinfo> GetreturnInfo(string name, IDbConnection conn)
        {
            return this.loanInfoAccess.GetReturnUserInfo(name, conn);
        }
    }
}
