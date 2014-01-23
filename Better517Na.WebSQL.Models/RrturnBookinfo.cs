//-----------------------------------------------------------------------
// <copyright file="RrturnBookinfo.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: RrturnBookinfo.cs
// * history : created by shaoyu 2014/1/19 18:53:10 
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
    /// 还书信息类
    /// </summary>
    public class RrturnBookinfo
    {
        /// <summary>
        /// 图书缴纳罚款状态
        /// </summary>
        private string bookPayStat = string.Empty;

        /// <summary>
        /// 缴纳罚款状态
        /// </summary>
        private int bookState = 0;

        /// <summary>
        /// 借阅人姓名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 图书条形码
        /// </summary>
        public Guid BookOrderNum
        {
            get;
            set;
        }

        /// <summary>
        /// 书名
        /// </summary>
        public string Booktitle
        {
            get;
            set;
        }

        /// <summary>
        /// 借书日期
        /// </summary>
        public DateTime BorrowDate
        {
            get;
            set;
        }

        /// <summary>
        /// 预计归还日期
        /// </summary>
        public DateTime ReturnDate
        {
            get;
            set;
        }

        /// <summary>
        /// 缴纳罚款状态
        /// </summary>
        public int BookState
        {
            get
            {
                return this.bookState;
            }

            set
            {
                this.bookState = value;
            }
        }

        /// <summary>
        /// 图书缴纳罚款状态
        /// </summary>
        public string BookPayStat
        {
            get
            {
                return this.bookPayStat;
            }

            set
            {
                this.bookPayStat = value;
            }
        }

        /// <summary>
        /// 罚款金额
        /// </summary>
        public decimal PayMoney
        {
            get;
            set;
        }
    }
}
