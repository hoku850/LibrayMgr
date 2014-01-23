//-----------------------------------------------------------------------
// <copyright file="GetBookNumByUserName.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: GetBookNumByUserName.cs
// * history : created by shaoyu 2014/1/21 20:55:24 
// </copyright>
//-----------------------------------------------------------------------
using System.Data;
using Better.Infrastructures.DBUtility;
using Better517Na.LibrayMgr.Factory;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    /// 查询该人借阅数量
    /// </summary>
    public class GetBookNumByUserName : DbTransaction
    {
        /// <summary>
        /// 存储借阅信息阅读器
        /// </summary>
        private IDAL.ILoanInfoAccess loanInfoAccess = null;

        /// <summary>
        /// 借阅数量
        /// </summary>
        private int borrowbokknum = 0;

        /// <summary>
        /// 借阅人姓名
        /// </summary>
        private string userName = string.Empty;

        /// <summary>
        /// 通过ID获取图书信息
        /// </summary>
        /// <param name="usernaem">借阅人姓名</param>
        public GetBookNumByUserName(string usernaem)
        {
            this.userName = usernaem;
            this.loanInfoAccess = DALFactory.CreateLoanInfoAccess();
            this.Connection = Better517Na.LibrayMgr.ConnectionFactory.ConnectionFactory.CreateConnect();
            this.IsBeginTransaction = false;
            this.IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 借阅数量
        /// </summary>
        public int Borrowbokknum
        {
            get
            {
                return this.borrowbokknum;
            }

            set
            {
                this.borrowbokknum = value;
            }
        }

        /// <summary>
        /// 获取图书信息
        /// </summary>
        protected override void ExecuteMethods()
        {
            this.borrowbokknum = this.GetbooknumbyUsername(this.userName, this.Connection);
        }

        /// <summary>
        /// 查询该人借阅数量
        /// </summary>
        /// <param name="name">借阅人姓名</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>借阅数量</returns>
        private int GetbooknumbyUsername(string name, IDbConnection conn)
        {
            int booknum = 0;
            booknum = this.loanInfoAccess.GetbooknumbyUsername(name, conn);
            return booknum;
        }
    }
}
