//-----------------------------------------------------------------------
// <copyright file="DALFactory.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: DALFactory.cs
// * history : created by shaoyu 2014/1/16 9:37:13 
// </copyright>
//-----------------------------------------------------------------------
using Better517Na.LibrayMgr.DAL;
using Better517Na.LibrayMgr.IDAL;

namespace Better517Na.LibrayMgr.Factory
{
    /// <summary>
    /// 数据库访问构建工厂类 
    /// </summary>
    public sealed class DALFactory
    {
        /// <summary>
        /// 构建图书信息数据库访问对象
        /// </summary>
        /// <returns>图书信息数据库访问对象</returns>
        public static IBookInfoAccess CreateBookInfoAccess()
        {
            return new BookInfoAccess();
        }

        /// <summary>
        /// 构建图书馆藏书数据库访问对象
        /// </summary>
        /// <returns>图书馆藏书数据库访问对象</returns>
        public static IBookItemAccess CreateBookItemAccess()
        {
            return new BookItemAccess();
        }

        /// <summary>
        /// 构建借阅情况数据库访问对象
        /// </summary>
        /// <returns>借阅情况数据库访问对象</returns>
        public static ILoanInfoAccess CreateLoanInfoAccess()
        {
            return new BookLoanInfoAccess();
        }
    }
}
