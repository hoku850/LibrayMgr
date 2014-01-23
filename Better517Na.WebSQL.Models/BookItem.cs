//-----------------------------------------------------------------------
// <copyright file="BookItem.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: BookItem.cs
// * history : created by shaoyu 2014/1/16 9:20:51 
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
    /// 图书藏书类 
    /// </summary>
    public class BookItem
    {
        /// <summary>
        /// 条形码
        /// </summary>
        public Guid OrderNum
        {
            get;
            set;
        }

        /// <summary>
        /// 图书ID
        /// </summary>
        public Guid BookID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否借出
        /// </summary>
        public int ISLoan
        {
            get;
            set;
        }
    }
}
