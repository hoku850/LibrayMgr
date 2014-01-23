// <copyright file="MysqlHelper.cs" company="517NA">
//   Copyright (c) 517na Enterprises. All rights reserved.
// </copyright>
// <author>陈冬</author>
// <create>2011-10-22</create>
//-----------------------------------------------------------------------

namespace Better.Infrastructures.DBUtility
{
    using System;
    using System.Data;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// <para>数据操作辅助类</para>
    /// <para>注意：使用完IDbConnection、IDbTransaction、MySqlDataReader等对象后应及时释放资源</para>
    /// </summary>
    public class MysqlHelper
    {
        #region " 构建 IDbConnection 对象 "
        /*
        /// <summary>
        /// 构建IDbConnection对象
        /// </summary>
        /// <param name="sqlConnectionString">IDbConnection连接字符串</param>
        /// <returns>IDbConnection对象</returns>
        public static IDbConnection CreateConnection(string sqlConnectionString)
        {
            if (string.IsNullOrEmpty(sqlConnectionString))
            {
                throw new ArgumentNullException("SqlConnectionString");
            }

            return new IDbConnection(sqlConnectionString);
        }

        /// <summary>
        /// 依据config配置文件中的sqlConnection Key 构建 IDbConnection对象
        /// </summary>
        /// <param name="name">sqlConnection Key</param>
        /// <returns>IDbConnection对象</returns>
        public static IDbConnection CreateConnectionBySettingsKey(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings[name];

            if (settings == null)
            {
                throw new Exception(string.Format("connectionStrings中没有name为{0}的设置", name));
            }

            return CreateConnection(settings.ConnectionString);
        }
        */
        #endregion

        #region " ExecuteScalar （获得首行首列） "

        /// <summary>
        /// 执行SQL获得首行首列
        /// </summary>
        /// <param name="trans">IDbTransaction对象</param> 
        /// <param name="sqlConnection">IDbConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">SQL脚本(sqltext)</param>
        /// <returns>执行SQL，首行首列数据值</returns>
        public static object ExecuteScalar(IDbTransaction trans, IDbConnection sqlConnection, string sqlString)
        {
            return ExecuteScalar(trans, sqlConnection, sqlString, null);
        }

        /// <summary>
        /// 执行SQL获得首行首列
        /// </summary>
        /// <param name="trans">IDbTransaction对象</param> 
        /// <param name="sqlConnection">IDbConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">SQL脚本（sqltext）</param>
        /// <param name="sqlParms">MySqlParameter参数集合</param>
        /// <returns>执行SQL，首行首列数据值</returns>
        public static object ExecuteScalar(IDbTransaction trans, IDbConnection sqlConnection, string sqlString, params MySqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlString))
            {
                throw new ArgumentNullException("sqlString");
            }

            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, trans, sqlString, 0, CommandType.Text, sqlParms);
                object result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return result;
            }
        }

        /// <summary>
        /// 执行存储过程获得首行首列数据
        /// </summary>
        /// <param name="sqlConnection">IDbConnection数据库连接对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行存储过程后获得首行首列数据</returns>
        public static object RunProcedureScalar(IDbConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedureScalar(sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程获得首行首列
        /// </summary>
        /// <param name="sqlConnection">IDbConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <param name="sqlParms">MySqlParameter参数集合</param>
        /// <returns>执行存储过程后获得首行首列数据</returns>
        public static object RunProcedureScalar(IDbConnection sqlConnection, string sqlProcedureName, params MySqlParameter[] sqlParms)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException("IDbConnection");
            }

            if (string.IsNullOrEmpty(sqlProcedureName))
            {
                throw new ArgumentNullException("sqlProcedureName");
            }

            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, null, sqlProcedureName, 0, CommandType.StoredProcedure, sqlParms);
                object result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return result;
            }
        }

        #endregion

        #region "ExecuteNonQuery （返回受影响的行数）  "

        /// <summary>
        /// 执行SQL脚本返回影响的行数
        /// </summary>
        /// <param name="trans">IDbTransaction 对象</param> 
        /// <param name="sqlConnection">IDbConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">SQL脚本(sqltext)</param>
        /// <returns>执行SQL脚本影响的行数</returns>
        public static int ExecuteSql(IDbTransaction trans, IDbConnection sqlConnection, string sqlString)
        {
            return ExecuteSql(trans, sqlConnection, sqlString, null);
        }

        /// <summary>
        /// 执行SQL脚本返回影响的行数
        /// </summary>
        /// <param name="trans">IDbTransaction 对象</param> 
        /// <param name="sqlConnection">IDbConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">SQL脚本(sqltext)</param>
        /// <param name="sqlParms">MySqlParameter参数集合</param>
        /// <returns>执行SQL脚本影响的行数</returns>
        public static int ExecuteSql(IDbTransaction trans, IDbConnection sqlConnection, string sqlString, params MySqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlString))
            {
                throw new ArgumentNullException("sqlString");
            }

            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, trans, sqlString, 0, CommandType.Text, sqlParms);
                int result = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return result;
            }
        }

        /// <summary>
        /// 执行存储过程返回受影响的行数
        /// </summary>
        /// <param name="sqlConnection">IDbConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行SQL脚本影响的行数</returns>
        public static int RunProcedure(IDbConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedure(sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程返回受影响的行数
        /// </summary>
        /// <param name="sqlConnection">IDbConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <param name="sqlParms">MySqlParameter对象集合</param>
        /// <returns>执行SQL脚本影响的行数</returns>
        public static int RunProcedure(IDbConnection sqlConnection, string sqlProcedureName, params MySqlParameter[] sqlParms)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException("IDbConnection");
            }

            if (string.IsNullOrEmpty(sqlProcedureName))
            {
                throw new ArgumentNullException("sqlProcedureName");
            }

            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, null, sqlProcedureName, 0, CommandType.StoredProcedure, sqlParms);
                int result = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return result;
            }
        }
        #endregion

        #region " MySqlDataReader （返回只读向前的数据集） "

        /// <summary>
        /// 执行Sql脚本返回 MySqlDataReader
        /// </summary>
        /// <param name="trans">IDbTransaction 对象</param>
        /// <param name="sqlConnection">IDbConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">Sql脚本（sqltext）</param>
        /// <returns>执行Sql数据库脚本，返回 MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(IDbTransaction trans, IDbConnection sqlConnection, string sqlString)
        {
            return ExecuteReader(trans, sqlConnection, sqlString, null);
        }

        /// <summary>
        /// 执行Sql脚本返回 MySqlDataReader
        /// </summary>
        /// <param name="trans">IDbTransaction 对象</param> 
        /// <param name="sqlConnection">IDbConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">Sql脚本(sqltext)</param>
        /// <param name="sqlParms">MySqlParameter 参数集合</param>
        /// <returns>执行Sql数据库脚本，返回 MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(IDbTransaction trans, IDbConnection sqlConnection, string sqlString, params MySqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlString))
            {
                throw new ArgumentNullException("sqlString");
            }

            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, trans, sqlString, 0, CommandType.Text, sqlParms);
                MySqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
        }

        /// <summary>
        /// 执行存储过程返回 MySqlDataReader
        /// </summary>
        /// <param name="sqlConnection">IDbConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行存储过程，返回 MySqlDataReader 对象</returns>
        public static MySqlDataReader RunProcedureReader(IDbConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedureReader(sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程返回 MySqlDataReader
        /// </summary>
        /// <param name="sqlConnection">IDbConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名（ProcedureName）</param>
        /// <param name="sqlParms">MySqlParameter 参数集合</param>
        /// <returns>执行存储过程，返回 MySqlDataReader对象</returns>
        public static MySqlDataReader RunProcedureReader(IDbConnection sqlConnection, string sqlProcedureName, params MySqlParameter[] sqlParms)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException("IDbConnection");
            }

            if (string.IsNullOrEmpty(sqlProcedureName))
            {
                throw new ArgumentNullException("sqlProcedureName");
            }

            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, null, sqlProcedureName, 0, CommandType.StoredProcedure, sqlParms);
                MySqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
        }

        #endregion

        #region " Query "

        /// <summary>
        /// 执行Sql脚本返回DataSet数据集合
        /// </summary>
        /// <param name="trans">IDbTransaction 对象</param>
        /// <param name="sqlConnection">IDbConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">Sql脚本(sqltext)</param>
        /// <returns>执行Sql数据库脚本，返回DataSet数据集合</returns>
        public static DataSet Query(IDbTransaction trans, IDbConnection sqlConnection, string sqlString)
        {
            return Query(trans, sqlConnection, sqlString, null);
        }

        /// <summary>
        /// 执行Sql脚本返回DataSet数据集合
        /// </summary>
        /// <param name="trans">IDbTransaction 对象</param>
        /// <param name="sqlConnection">IDbConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">Sql脚本（sqltext）</param>
        /// <param name="sqlParms">SqlParameter 参数集合</param>
        /// <returns>执行Sql数据库脚本，返回DataSet数据集合</returns>
        public static DataSet Query(IDbTransaction trans, IDbConnection sqlConnection, string sqlString, params MySqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlString))
            {
                throw new ArgumentNullException("sqlString");
            }

            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, trans, sqlString, 0, CommandType.Text, sqlParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet objDs = new DataSet();
                    da.Fill(objDs, "objDs");
                    return objDs;
                }
            }
        }

        /// <summary>
        /// 执行存储过程返回DataSet数据集合
        /// </summary>
        /// <param name="sqlConnection">IDbConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行数据库存储过程，返回DataSet数据集合</returns>
        public static DataSet RunProcedureQuery(IDbConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedureQuery(sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程返回DataSet数据集合
        /// </summary>
        /// <param name="sqlConnection">IDbConnection对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <param name="sqlParms">SqlParameter参数集合</param>
        /// <returns>执行数据库存储过程，返回DataSet数据集合</returns>
        public static DataSet RunProcedureQuery(IDbConnection sqlConnection, string sqlProcedureName, params MySqlParameter[] sqlParms)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException("IDbConnection");
            }

            if (string.IsNullOrEmpty(sqlProcedureName))
            {
                throw new ArgumentNullException("sqlProcedureName");
            }

            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, null, sqlProcedureName, 0, CommandType.StoredProcedure, sqlParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet objDs = new DataSet();
                    da.Fill(objDs, "objDs");
                    return objDs;
                }
            }
        }

        #endregion

        #region " 辅助方法  "

        /// <summary>
        /// 设置 MySqlCommand 对象属性
        /// </summary>
        /// <param name="cmd">MySqlCommand 对象</param>
        /// <param name="conn">IDbConnection 对象（连接）</param>
        /// <param name="trans">IDbTransaction 对象（事务）</param>
        /// <param name="cmdText">sql文本 或存储过程名称（ProcedureName）</param>
        /// <param name="timeOut">MySqlCommand 对象超时时间</param>
        /// <param name="cmdType">MySqlCommand 类型</param>
        /// <param name="sqlParameter">MySqlParameter（参数） 集合</param>
        private static void PrepareCommand(MySqlCommand cmd, IDbConnection conn, IDbTransaction trans, string cmdText, int timeOut, CommandType cmdType, MySqlParameter[] sqlParameter)
        {
            if (trans != null && conn != null)
            {
                throw new ArgumentException("IDbTransaction 和 IDbConnection　其一必须为null");
            }

            if (trans != null)
            {
                cmd.Transaction = trans as MySqlTransaction;
                conn = trans.Connection;
            }

            if (conn == null)
            {
                throw new ArgumentNullException("IDbConnection");
            }

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.CommandType = cmdType;

            if (timeOut > 0)
            {
                cmd.CommandTimeout = timeOut;
            }

            cmd.Connection = conn as MySqlConnection;
            cmd.CommandText = cmdText;
            if (sqlParameter != null)
            {
                foreach (MySqlParameter parameter in sqlParameter)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion
    }
}