//-----------------------------------------------------------------------
// <copyright file="OutTimeBookInfo.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: OutTimeBookInfo.cs
// * history : created by shaoyu 2014/1/20 11:58:33 
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
    /// 超期图书信息
    /// </summary>
    public class OutTimeBookInfo
    {
        /// <summary>
        /// 借书日期
        /// </summary>
        private DateTime borrowDate = new DateTime();

        /// <summary>
        /// 借书至今的天数
        /// </summary>
        private int borrowDays = 0;

        /// <summary>
        /// 应该缴纳 的罚款数
        /// </summary>
        private double money = 0;

        /// <summary>
        /// 应该缴纳 的罚款数
        /// </summary>
        public double Money
        {
            get 
            { 
                return this.money;
            }

            set 
            {
                this.money = value;
            }
        }

        /// <summary>
        /// 借阅人姓名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 借书至今的天数
        /// </summary>
        public int BorrowDays
        {
            get
            {
                return this.borrowDays;
            }

            set
            {
                this.borrowDays = value;
            }
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
            get
            {
                return this.borrowDate;
            }

            set
            {
                // 通过借书日期计算超期天数和罚款数目
                DateTime tadytime = new DateTime();
                tadytime = DateTime.Now;
                this.borrowDate = value;
                this.BorrowDays = (tadytime - this.borrowDate).Days;
                if (this.BorrowDays > 30)
                {
                    this.Money = (this.BorrowDays - 30) * 0.5;
                }
            }
        }
    }
}
