// <copyright file="SqlHelper.cs" company="517na.com">
//     Copyright (c) 517na.com. All rights reserved.
// </copyright>

namespace Better.Infrastructures.DBUtility
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// <para>数据操作辅助类</para>
    /// <para>注意：使用完SqlConnection、SqlTransaction、SqlDataReader等对象后应及时释放资源</para>
    /// </summary>
    public class SqlHelper
    {
        #region " 构建 SqlConnection 对象 "
        /*
        /// <summary>
        /// 构建SqlConnection对象
        /// </summary>
        /// <param name="sqlConnectionString">SqlConnection连接字符串</param>
        /// <returns>SqlConnection对象</returns>
        public static SqlConnection CreateConnection(string sqlConnectionString)
        {
            if (string.IsNullOrEmpty(sqlConnectionString))
            {
                throw new ArgumentNullException("SqlConnectionString");
            }

            return new SqlConnection(sqlConnectionString);
        }

        /// <summary>
        /// 依据config配置文件中的sqlConnection Key 构建 SqlConnection对象
        /// </summary>
        /// <param name="name">sqlConnection Key</param>
        /// <returns>SqlConnection对象</returns>
        public static SqlConnection CreateConnectionBySettingsKey(string name)
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
        /// <param name="trans">SqlTransaction对象</param> 
        /// <param name="sqlConnection">SqlConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">SQL脚本(sqltext)</param>
        /// <returns>执行SQL，首行首列数据值</returns>
        public static object ExecuteScalar(SqlTransaction trans, SqlConnection sqlConnection, string sqlString)
        {
           
            return ExecuteScalar(trans, sqlConnection, sqlString, null);
        }

        /// <summary>
        /// 执行SQL获得首行首列
        /// </summary>
        /// <param name="trans">SqlTransaction对象</param> 
        /// <param name="sqlConnection">SqlConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">SQL脚本（sqltext）</param>
        /// <param name="sqlParms">SqlParameter参数集合</param>
        /// <returns>执行SQL，首行首列数据值</returns>
        public static object ExecuteScalar(SqlTransaction trans, SqlConnection sqlConnection, string sqlString, params SqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlString))
            {
                throw new ArgumentNullException("sqlString");
            }

            using (SqlCommand cmd = new SqlCommand())
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
        /// <param name="sqlConnection">SqlConnection数据库连接对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行存储过程后获得首行首列数据</returns>
        public static object RunProcedureScalar(SqlConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedureScalar(sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程获得首行首列
        /// </summary>
        /// <param name="sqlConnection">SqlConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <param name="sqlParms">SqlParameter参数集合</param>
        /// <returns>执行存储过程后获得首行首列数据</returns>
        public static object RunProcedureScalar(SqlConnection sqlConnection, string sqlProcedureName, params SqlParameter[] sqlParms)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException("SqlConnection");
            }

            return RunProcedureScalar(null, sqlConnection, sqlProcedureName, sqlParms);
        }

        /// <summary>
        /// 执行存储过程获得首行首列数据
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="sqlConnection">SqlConnection数据库连接对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行存储过程后获得首行首列数据</returns>
        public static object RunProcedureScalar(SqlTransaction trans, SqlConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedureScalar(trans, sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程获得首行首列
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="sqlConnection">连接对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <param name="sqlParms">SqlParameter参数集合</param>
        /// <returns>执行存储过程后获得首行首列数据</returns>
        public static object RunProcedureScalar(SqlTransaction trans, SqlConnection sqlConnection, string sqlProcedureName, params SqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlProcedureName))
            {
                throw new ArgumentNullException("sqlProcedureName");
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, trans, sqlProcedureName, 0, CommandType.StoredProcedure, sqlParms);
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
        /// <param name="trans">SqlTransaction 对象</param> 
        /// <param name="sqlConnection">SqlConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">SQL脚本(sqltext)</param>
        /// <returns>执行SQL脚本影响的行数</returns>
        public static int ExecuteSql(SqlTransaction trans, SqlConnection sqlConnection, string sqlString)
        {
            return ExecuteSql(trans, sqlConnection, sqlString, null);
        }

        /// <summary>
        /// 执行SQL脚本返回影响的行数
        /// </summary>
        /// <param name="trans">SqlTransaction 对象</param> 
        /// <param name="sqlConnection">SqlConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">SQL脚本(sqltext)</param>
        /// <param name="sqlParms">SqlParameter参数集合</param>
        /// <returns>执行SQL脚本影响的行数</returns>
        public static int ExecuteSql(SqlTransaction trans, SqlConnection sqlConnection, string sqlString, params SqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlString))
            {
                throw new ArgumentNullException("sqlString");
            }

            using (SqlCommand cmd = new SqlCommand())
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
        /// <param name="sqlConnection">SqlConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行SQL脚本影响的行数</returns>
        public static int RunProcedure(SqlConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedure(sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程返回受影响的行数
        /// </summary>
        /// <param name="sqlConnection">SqlConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <param name="sqlParms">SqlParameter对象集合</param>
        /// <returns>执行SQL脚本影响的行数</returns>
        public static int RunProcedure(SqlConnection sqlConnection, string sqlProcedureName, params SqlParameter[] sqlParms)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException("SqlConnection");
            }

            if (string.IsNullOrEmpty(sqlProcedureName))
            {
                throw new ArgumentNullException("sqlProcedureName");
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, null, sqlProcedureName, 0, CommandType.StoredProcedure, sqlParms);
                int result = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return result;
            }
        }
        #endregion

        #region " SqlDataReader （返回只读向前的数据集） "

        /// <summary>
        /// 执行Sql脚本返回 SqlDataReader
        /// </summary>
        /// <param name="trans">SqlTransaction 对象</param>
        /// <param name="sqlConnection">SqlConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">Sql脚本（sqltext）</param>
        /// <returns>执行Sql数据库脚本，返回 SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction trans, SqlConnection sqlConnection, string sqlString)
        {
            return ExecuteReader(trans, sqlConnection, sqlString, null);
        }

        /// <summary>
        /// 执行Sql脚本返回 SqlDataReader
        /// </summary>
        /// <param name="trans">SqlTransaction 对象</param> 
        /// <param name="sqlConnection">SqlConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">Sql脚本(sqltext)</param>
        /// <param name="sqlParms">SqlParameter 参数集合</param>
        /// <returns>执行Sql数据库脚本，返回 SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction trans, SqlConnection sqlConnection, string sqlString, params SqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlString))
            {
                throw new ArgumentNullException("sqlString");
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, trans, sqlString, 0, CommandType.Text, sqlParms);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
        }

        /// <summary>
        /// 执行存储过程返回 SqlDataReader
        /// </summary>
        /// <param name="sqlConnection">SqlConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行存储过程，返回 SqlDataReader 对象</returns>
        public static SqlDataReader RunProcedureReader(SqlConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedureReader(sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程返回 SqlDataReader
        /// </summary>
        /// <param name="sqlConnection">SqlConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名（ProcedureName）</param>
        /// <param name="sqlParms">SqlParameter 参数集合</param>
        /// <returns>执行存储过程，返回 SqlDataReader对象</returns>
        public static SqlDataReader RunProcedureReader(SqlConnection sqlConnection, string sqlProcedureName, params SqlParameter[] sqlParms)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException("SqlConnection");
            }

            if (string.IsNullOrEmpty(sqlProcedureName))
            {
                throw new ArgumentNullException("sqlProcedureName");
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, null, sqlProcedureName, 0, CommandType.StoredProcedure, sqlParms);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
        }

        #endregion

        #region " Query "
       
        /// <summary>
        /// 执行Sql脚本返回DataSet数据集合
        /// </summary>
        /// <param name="trans">SqlTransaction 对象</param>
        /// <param name="sqlConnection">SqlConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">Sql脚本(sqltext)</param>
        /// <returns>执行Sql数据库脚本，返回DataSet数据集合</returns>
        public static DataSet Query(SqlTransaction trans, SqlConnection sqlConnection, string sqlString)
        {
            return Query(trans, sqlConnection, sqlString, null);
        }

        /// <summary>
        /// 执行Sql脚本返回DataSet数据集合
        /// </summary>
        /// <param name="trans">SqlTransaction 对象</param>
        /// <param name="sqlConnection">SqlConnection对象，使用事务时候可不传递</param>
        /// <param name="sqlString">Sql脚本（sqltext）</param>
        /// <param name="sqlParms">SqlParameter 参数集合</param>
        /// <returns>执行Sql数据库脚本，返回DataSet数据集合</returns>
        public static DataSet Query(SqlTransaction trans, SqlConnection sqlConnection, string sqlString, params SqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlString))
            {
                throw new ArgumentNullException("sqlString");
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, trans, sqlString, 0, CommandType.Text, sqlParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
        /// <param name="sqlConnection">SqlConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行数据库存储过程，返回DataSet数据集合</returns>
        public static DataSet RunProcedureQuery(SqlConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedureQuery(sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程返回DataSet数据集合
        /// </summary>
        /// <param name="sqlConnection">SqlConnection对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <param name="sqlParms">SqlParameter参数集合</param>
        /// <returns>执行数据库存储过程，返回DataSet数据集合</returns>
        public static DataSet RunProcedureQuery(SqlConnection sqlConnection, string sqlProcedureName, params SqlParameter[] sqlParms)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException("SqlConnection");
            }

            return RunProcedureQuery(null, sqlConnection, sqlProcedureName, sqlParms);
        }

        /// <summary>
        /// 执行存储过程返回DataSet数据集合
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="sqlConnection">SqlConnection 对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <returns>执行数据库存储过程，返回DataSet数据集合</returns>
        public static DataSet RunProcedureQuery(SqlTransaction trans, SqlConnection sqlConnection, string sqlProcedureName)
        {
            return RunProcedureQuery(trans, sqlConnection, sqlProcedureName, null);
        }

        /// <summary>
        /// 执行存储过程返回DataSet数据集合
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="sqlConnection">SqlConnection对象</param>
        /// <param name="sqlProcedureName">存储过程名称（ProcedureName）</param>
        /// <param name="sqlParms">SqlParameter参数集合</param>
        /// <returns>执行数据库存储过程，返回DataSet数据集合</returns>
        public static DataSet RunProcedureQuery(SqlTransaction trans, SqlConnection sqlConnection, string sqlProcedureName, params SqlParameter[] sqlParms)
        {
            if (string.IsNullOrEmpty(sqlProcedureName))
            {
                throw new ArgumentNullException("sqlProcedureName");
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, sqlConnection, trans, sqlProcedureName, 0, CommandType.StoredProcedure, sqlParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
        /// 设置 SqlCommand 对象属性
        /// </summary>
        /// <param name="cmd">SqlCommand 对象</param>
        /// <param name="conn">SqlConnection 对象（连接）</param>
        /// <param name="trans">SqlTransaction 对象（事务）</param>
        /// <param name="cmdText">sql文本 或存储过程名称（ProcedureName）</param>
        /// <param name="timeOut">SqlCommand 对象超时时间</param>
        /// <param name="cmdType">SqlCommand 类型</param>
        /// <param name="sqlParameter">SqlParameter（参数） 集合</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, int timeOut, CommandType cmdType, SqlParameter[] sqlParameter)
        {
            if (trans != null && conn != null)
            {
                throw new ArgumentException("SqlTransaction 和 SqlConnection　其一必须为null");
            }

            if (trans != null)
            {
                cmd.Transaction = trans;
                conn = trans.Connection;
            }

            if (conn == null)
            {
                throw new ArgumentNullException("SqlConnection");
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

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (sqlParameter != null)
            {
                foreach (SqlParameter parameter in sqlParameter)
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
