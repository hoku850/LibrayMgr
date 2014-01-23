//-----------------------------------------------------------------------
// <copyright file="UerInfo.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: UerInfo.cs
// * history : created by shaoyu 2014/1/18 9:06:43 
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
    /// UerInfo
    /// </summary>
    public class UerInfo : List<Guid>
    {
        /// <summary>
        /// 是否可继续借书
        /// </summary>
        public bool Isborrow
        {
            get;
            set;
        }

        /// <summary>
        /// 超期数目
        /// </summary>
        public int OutBack
        {
            get;
            set;
        }

        /// <summary>
        /// 已借数目
        /// </summary>
        public int Booknum
        {
            get;
            set;
        }
    }
}
