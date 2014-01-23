//-----------------------------------------------------------------------
// <copyright file="LoanInfo.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: LoanInfo.cs
// * history : created by shaoyu 2014/1/16 8:55:06 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Better517Na.LibrayMgr.Models
{
    /// <summary>
    /// 存储借阅信息类
    /// </summary>
    public class LoanInfo
    {
        /// <summary>
        /// 借阅ID
        /// </summary>
        public Guid LoanID
        {
            get;
            set;
        }

        /// <summary>
        /// 借阅人
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 图书条形码
        /// </summary>
        public Guid OrderNum
        {
            get;
            set;
        }

        /// <summary>
        /// 借阅时间
        /// </summary>
        public DateTime LoanDate
        {
            get;
            set;
        }
    }
}
