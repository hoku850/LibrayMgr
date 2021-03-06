﻿//-----------------------------------------------------------------------
// <copyright file="Bookinfo.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: Bookinfo.cs
// * history : created by shaoyu 2014/1/16 9:20:21 
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
    /// 图书信息类
    /// </summary>
    public class Bookinfo
    {
        /// <summary>
        /// 图书ID
        /// </summary>
        public Guid BookID
        {
            get;
            set;
        }

        /// <summary>
        /// 图书名字
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 图书描述
        /// </summary>
        public string Decrible
        {
            get;
            set;
        }
    }
}
