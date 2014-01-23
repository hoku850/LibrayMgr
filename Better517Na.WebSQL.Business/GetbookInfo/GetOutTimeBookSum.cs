//-----------------------------------------------------------------------
// <copyright file="GetOutTimeBookSum.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetOutTimeBookSum.cs
// * history : created by shaoyu 2014/1/21 21:14:55 
// </copyright>
//-----------------------------------------------------------------------
using System.Data;
using Better.Infrastructures.DBUtility;
using Better517Na.LibrayMgr.Factory;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    ///  查询该人借书超期数量
    /// </summary>
    public class GetOutTimeBookSum : DbTransaction
    {
        /// <summary>
        /// 存储借阅信息阅读器
        /// </summary>
        private IDAL.ILoanInfoAccess loanInfoAccess = null;

        /// <summary>
        /// 借阅人信息
        /// </summary>
        private UerInfo uerInfo = new UerInfo();

        /// <summary>
        /// 借阅人姓名
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// 查询该人借书超期数量构造器
        /// </summary>
        /// <param name="usernaem">借阅人姓名</param>
        public GetOutTimeBookSum(string usernaem)
        {
            this.userName = usernaem;
            this.loanInfoAccess = DALFactory.CreateLoanInfoAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 超期信息
        /// </summary>
        public UerInfo UerInfo
        {
            get 
            {
                return this.uerInfo;
            }

            set 
            {
                this.uerInfo = value;
            }
        }

        /// <summary>
        /// 查询该人借书超期数量
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.uerInfo = this.GetOutTimebooksum(this.userName, this.Connection);
        }

        /// <summary>
        /// 查询该人借书超期数量
        /// </summary>
        /// <param name="name">借阅人姓名</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>超期数量</returns>
        private UerInfo GetOutTimebooksum(string name, IDbConnection conn)
        {
            UerInfo userinfo = new UerInfo();
            userinfo = this.loanInfoAccess.GetOutTimebooksum(name, conn);
            return userinfo;
        }
    }
}
