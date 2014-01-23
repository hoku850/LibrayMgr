//-----------------------------------------------------------------------
// <copyright file="BookLoanInfoAccess.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: BookLoanInfoAccess.cs
// * history : created by shaoyu 2014/1/17 16:54:02 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Better.Infrastructures.DBUtility;
using Better.Infrastructures.Log;
using Better517Na.LibrayMgr.IDAL;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.DAL
{
    /// <summary>
    /// 借阅信息查询类
    /// </summary>
    public class BookLoanInfoAccess : ILoanInfoAccess
    {
        /// <summary>
        /// 查询该人借阅数量
        /// </summary>
        /// <param name="name">借阅人姓名</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>借阅数量</returns>
        public int GetbooknumbyUsername(string name, IDbConnection conn)
        {
            object obj = null;
            try
            {
                string sqlforbookitem = "SELECT COUNT(LoanID)  FROM LoanInfo WHERE UserName= @name and ReturnDate IS  NULL";
                SqlParameter[] parameters = { new SqlParameter("@name", SqlDbType.NVarChar, 50) };
                parameters[0].Value = name;
                obj = SqlHelper.ExecuteScalar(null, conn as SqlConnection, sqlforbookitem, parameters);
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("查询该人借阅数量", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return (int)obj;
        }

        /// <summary>
        /// 查询该人借书超期数量
        /// </summary>
        /// <param name="name">借阅人姓名</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>超期信息</returns>
        public UerInfo GetOutTimebooksum(string name, IDbConnection conn)
        {
            UerInfo userinfo = new UerInfo();
            try
            {
                string sqlforbookitem = "SELECT LoanDate, OrderNum FROM LoanInfo WHERE UserName= @name";
                SqlParameter[] parameters = { new SqlParameter("@name", SqlDbType.NVarChar, 50) };
                parameters[0].Value = name;

                // 统计数量
                using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqlforbookitem, parameters))
                {
                    if (result.HasRows)
                    {
                        DateTime tadytime = DateTime.Now;
                        while (result.Read())
                        {
                            DateTime borrowtime = result.GetDateTime(0);
                            Guid orderid = result.GetGuid(1);
                            if ((tadytime - borrowtime).Days >= 30)
                            {
                                userinfo.OutBack++;
                                userinfo.Add(orderid);
                            }
                        }
                    }

                    result.Close();
                }
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("查询该人借书超期数量", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return userinfo;
        }

        /// <summary>
        /// 查询借书人 已还图书中还未付清罚款的信息
        /// </summary>
        /// <param name="name">借书人</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>欠款信息</returns>
        public List<RrturnBookinfo> GetReturnUserInfo(string name, IDbConnection conn)
        {
            string sqltext = @"select LoanInfo.UserName, LoanInfo.OrderNum,BookInfo.Title, LoanInfo.LoanDate , LoanInfo.ReturnDate, LoanInfo.Fine FROM LoanInfo INNER JOIN BookItem  ON LoanInfo.OrderNum = BookItem.OrderNum INNER JOIN BookInfo ON BookItem.BookID = BookInfo.BookID  where LoanInfo.UserName = @Username  and LoanInfo.IsPaied = 0 and LoanInfo.ReturnDate IS NOT NULL";
            SqlParameter[] parameters = { new SqlParameter("@Username", SqlDbType.VarChar, 50) };
            parameters[0].Value = name;
            List<RrturnBookinfo> bookinfoList = new List<RrturnBookinfo>();
            using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqltext, parameters))
            {
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        RrturnBookinfo rturnBookinfo = new RrturnBookinfo();
                       
                        // 借阅人账号
                        if (!result.IsDBNull(0))
                        {
                            rturnBookinfo.UserName = result.GetString(0);
                        }

                        // 图书条形码
                        if (!result.IsDBNull(1))
                        {
                            rturnBookinfo.BookOrderNum = result.GetGuid(1);
                        }

                        // 图书名称
                        if (!result.IsDBNull(2))
                        {
                            rturnBookinfo.Booktitle = result.GetString(2);
                        }

                        // 借阅日期
                        if (!result.IsDBNull(3))
                        {
                            rturnBookinfo.BorrowDate = result.GetDateTime(3);
                        }

                        // 还书日期
                        if (!result.IsDBNull(4))
                        {
                            rturnBookinfo.ReturnDate = result.GetDateTime(4);
                        }

                        // 罚款金额
                        if (!result.IsDBNull(5))
                        {
                            rturnBookinfo.PayMoney = result.GetDecimal(5);
                        }

                        bookinfoList.Add(rturnBookinfo);
                    }
                }

                result.Close();
            }

            return bookinfoList;
        }

        /// <summary>
        /// 获取(归还/未归还)图书信息
        /// </summary>
        /// <param name="isreturn">是否归还</param>
        /// <param name="bookguid">书di</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>该书信息</returns>
        public RrturnBookinfo GetReturnBookInfo(bool isreturn, Guid bookguid, IDbConnection conn)
        {
            string sqltext = @"select LoanInfo.UserName, LoanInfo.OrderNum,BookInfo.Title, LoanInfo.LoanDate , LoanInfo.ReturnDate, LoanInfo.Fine FROM LoanInfo INNER JOIN BookItem  ON LoanInfo.OrderNum = BookItem.OrderNum INNER JOIN BookInfo ON BookItem.BookID = BookInfo.BookID  where LoanInfo.OrderNum = @bookguid ";
            StringBuilder sqlsb = new StringBuilder(sqltext);
            if (isreturn)
            {
                // 获取归还的图书
                sqlsb.Append(" and LoanInfo.ReturnDate IS NOT NULL");
            }
            else
            {
                // 获取未归还的图书
                sqlsb.Append(" and LoanInfo.ReturnDate IS  NULL");
            }

            SqlParameter[] parameters = { new SqlParameter("@bookguid", SqlDbType.UniqueIdentifier) };
            parameters[0].Value = bookguid;
            RrturnBookinfo rturnBookinfo = new RrturnBookinfo();
            using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqlsb.ToString(), parameters))
            {
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        // 借阅人账号
                        if (!result.IsDBNull(0))
                        {
                            rturnBookinfo.UserName = result.GetString(0);
                        }

                        // 图书条形码
                        if (!result.IsDBNull(1))
                        {
                            rturnBookinfo.BookOrderNum = result.GetGuid(1);
                        }

                        // 图书名称
                        if (!result.IsDBNull(2))
                        {
                            rturnBookinfo.Booktitle = result.GetString(2);
                        }

                        // 借阅日期
                        if (!result.IsDBNull(3))
                        {
                            rturnBookinfo.BorrowDate = result.GetDateTime(3);
                        }

                        if (isreturn)
                        {
                            // 还书日期
                            if (!result.IsDBNull(4))
                            {
                                rturnBookinfo.ReturnDate = result.GetDateTime(4);
                            }
                        }

                        // 罚款金额
                        if (!result.IsDBNull(5))
                        {
                            rturnBookinfo.PayMoney = result.GetDecimal(5);
                        }
                    }
                }

                result.Close();
            }

            return rturnBookinfo;
        }

        /// <summary>
        /// 插入借书信息
        /// </summary>
        /// <param name="orderNum">图书条形码</param>
        /// <param name="userName">借阅人姓名</param>
        /// <param name="trans">事务</param>
        /// <returns>影响数据库行数</returns>
        public int BorrowBook(Guid orderNum, string userName, IDbTransaction trans)
        {
            int result = 0;
            try
            {
                string sqlforbookitem = "INSERT INTO LoanInfo ( OrderNum,UserName) VALUES (@orderNum,@userName)";
                SqlParameter[] parameters = { new SqlParameter("@orderNum", SqlDbType.UniqueIdentifier), new SqlParameter("@userName", SqlDbType.VarChar, 50) };
                parameters[0].Value = orderNum;
                parameters[1].Value = userName;
                result = SqlHelper.ExecuteSql(trans as SqlTransaction, null, sqlforbookitem, parameters);
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("插入借书信息", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return result;
        }

        /// <summary>
        /// 付清罚款
        /// </summary>
        /// <param name="name">借书人</param>
        /// <param name="trans">事务</param>
        /// <returns>返回影响的行数</returns>
        public int PayMoney(string name, IDbTransaction trans)
        {
            int result = -1;

            // 将是否付款置1，罚款清零！
            string sqltext = @"update Top(5) LoanInfo set IsPaied = 1 where UserName = @Username";
            SqlParameter[] parameters = { new SqlParameter("@userName", SqlDbType.VarChar, 50) };
            parameters[0].Value = name;
            result = SqlHelper.ExecuteSql(trans as SqlTransaction, null, sqltext, parameters);
            return result;
        }

        /// <summary>
        /// 还书，只将还书日期添上（不缴纳罚款）
        /// </summary>
        /// <param name="bookguid">图书条形码</param>
        /// <param name="trans">事务</param>
        /// <returns>返回影响的行数</returns>
        public int ReturnBook(Guid bookguid, IDbTransaction trans)
        {
            int result = -1;
            DateTime gettime = new DateTime();

            // 获取还书时间
            gettime = DateTime.Now;
            string sqltext = @"update Top(1) LoanInfo set ReturnDate = @returndate where OrderNum = @bookguid";
            SqlParameter[] parameters = { new SqlParameter("@returndate", SqlDbType.DateTime), new SqlParameter("@bookguid", SqlDbType.UniqueIdentifier) };
            parameters[0].Value = gettime;
            parameters[1].Value = bookguid;
            result = SqlHelper.ExecuteSql(trans as SqlTransaction, null, sqltext, parameters);
            return result;
        }

        /// <summary>
        /// 缴纳罚款置1
        /// </summary>
        /// <param name="bookguid">超期图书条形码</param>
        /// <param name="trans">事务</param>
        /// <returns>影响的行数</returns>
        public int PayMoneyByguid(Guid bookguid, IDbTransaction trans)
        {
            int result = -1;
            string sqltext = @"update Top(1) LoanInfo set IsPaied = 1 where OrderNum = @bookguid";
            SqlParameter[] parameters = { new SqlParameter("@bookguid", SqlDbType.UniqueIdentifier) };
            parameters[0].Value = bookguid;
            result = SqlHelper.ExecuteSql(trans as SqlTransaction, null, sqltext, parameters);
            return result;
        }

        /// <summary>
        /// 写入罚款
        /// </summary>
        /// <param name="bookguid">借书ID</param>
        /// <param name="money">罚款金额</param>
        /// <param name="trans">事务</param>
        /// <returns>影响的行数</returns>
        public int WritePaied(Guid bookguid, decimal money, IDbTransaction trans)
        {
            int result = -1;
            string sqltext = @"update Top(1) LoanInfo set Fine = @money where OrderNum = @bookguid";
            SqlParameter[] parameters = { new SqlParameter("@money", SqlDbType.Decimal), new SqlParameter("@bookguid", SqlDbType.UniqueIdentifier) };
            parameters[0].Value = money;
            parameters[1].Value = bookguid;
            result = SqlHelper.ExecuteSql(trans as SqlTransaction, null, sqltext, parameters);
            return result;
        }

        /// <summary>
        /// 获取借阅信息
        /// </summary>
        /// <param name="isback">是否已还</param>
        /// <param name="username">用户id</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>借阅信息表</returns>
        public List<RrturnBookinfo> GetBorrowBookInfo(bool isback, string username, IDbConnection conn)
        {
            string sqltext = @"select LoanInfo.UserName, LoanInfo.OrderNum,BookInfo.Title, LoanInfo.LoanDate , LoanInfo.ReturnDate, LoanInfo.IsPaied, LoanInfo.Fine FROM LoanInfo INNER JOIN BookItem  ON LoanInfo.OrderNum = BookItem.OrderNum INNER JOIN BookInfo ON BookItem.BookID = BookInfo.BookID  where LoanInfo.UserName = @username";
            StringBuilder sqlsb = new StringBuilder(sqltext);
            if (isback)
            {
                // 获取归还的图书
                sqlsb.Append(" and LoanInfo.ReturnDate IS NOT NULL");
            }
            else
            {
                // 获取未归还的图书
                sqlsb.Append(" and LoanInfo.ReturnDate IS  NULL");
            }

            SqlParameter[] parameters = { new SqlParameter("@username", SqlDbType.VarChar, 50) };
            parameters[0].Value = username;
            List<RrturnBookinfo> rturnBookinfolist = new List<RrturnBookinfo>();
            using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqlsb.ToString(), parameters))
            {
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        RrturnBookinfo rturnBookinfo = new RrturnBookinfo();
                        
                        // 借阅人账号
                        if (!result.IsDBNull(0))
                        {
                            rturnBookinfo.UserName = result.GetString(0);
                        }

                        // 图书条形码
                        if (!result.IsDBNull(1))
                        {
                            rturnBookinfo.BookOrderNum = result.GetGuid(1);
                        }

                        // 图书名称
                        if (!result.IsDBNull(2))
                        {
                            rturnBookinfo.Booktitle = result.GetString(2);
                        }

                        // 借阅日期
                        if (!result.IsDBNull(3))
                        {
                            rturnBookinfo.BorrowDate = result.GetDateTime(3);
                        }

                        if (isback)
                        {
                            // 还书日期
                            if (!result.IsDBNull(4))
                            {
                                rturnBookinfo.ReturnDate = result.GetDateTime(4);
                            }
                        }
                        else
                        {
                            rturnBookinfo.ReturnDate = rturnBookinfo.BorrowDate.AddDays(30);
                        }

                        // 罚款状态
                        if (!result.IsDBNull(5))
                        {
                            rturnBookinfo.BookState = result.GetBoolean(5) ? 1 : 0;
                        }

                        // 罚款金额
                        if (!result.IsDBNull(6))
                        {
                            rturnBookinfo.PayMoney = result.GetDecimal(6);
                        }

                        rturnBookinfolist.Add(rturnBookinfo);
                    }
                }

                result.Close();
            }

            return rturnBookinfolist;
        }
    }
}
