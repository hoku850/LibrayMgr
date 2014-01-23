//-----------------------------------------------------------------------
// <copyright file="ConnetionStringConst.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: ConnetionStringConst.cs
// * history : created by shaoyu 2014/1/16 9:34:48 
// </copyright>
//-----------------------------------------------------------------------

namespace Better517Na.LibrayMgr.ConnectionFactory
{
    /// <summary>
    /// 数据库连接字符串类 
    /// </summary>
    public class ConnetionStringConst
    {
        /// <summary>
        /// ConnectString
        /// </summary>
        public static readonly string ConnectString = string.Empty;

        /// <summary>
        /// 构造函数静态初始划函数
        /// </summary>
        static ConnetionStringConst()
        {
            ConnectString = GetAppSettingsString("connectString");
        }

        /// <summary>
        /// 获得 .config 配置文件中 AppSettings 配置节信息
        /// </summary>
        /// <param name="name">ConnectionStrings 配置节的 key</param>
        /// <returns>SqlConnection 连接字符串</returns>
        private static string GetAppSettingsString(string name)
        {
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }
    }
}
