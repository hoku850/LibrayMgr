//-----------------------------------------------------------------------
// <copyright file="ConnectionFactory.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: ConnectionFactory.cs
// * history : created by shaoyu 2014/1/16 9:34:19 
// </copyright>
//-----------------------------------------------------------------------
using System.Data;
using System.Data.SqlClient;

namespace Better517Na.LibrayMgr.ConnectionFactory
{
    /// <summary>
    /// 数据库连接对象创建工厂类 
    /// </summary>
    public class ConnectionFactory
    {
        /// <summary>
        /// 构建数据库数据库连接对象（IDbConnection）
        /// </summary>
        /// <returns>数据库（User）数据库连接对象（IDbConnection）</returns>
        public static IDbConnection CreateConnect()
        {
            return new SqlConnection(ConnetionStringConst.ConnectString);
        }
    }
}
