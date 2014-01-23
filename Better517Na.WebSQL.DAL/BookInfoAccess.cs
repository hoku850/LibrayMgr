//-----------------------------------------------------------------------
// <copyright file="BookInfoAccess.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: BookInfoAccess.cs
// * history : created by shaoyu 2014/1/17 13:51:08 
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
    /// 查询图书信息类
    /// </summary>
    public class BookInfoAccess : IBookInfoAccess
    {
        /// <summary>
        /// 根据图书ID查询图书信息
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>图书信息</returns>
        public Bookinfo GetBookInfoBybookid(Guid bookID, IDbConnection conn)
        {
            Bookinfo bookinfo = new Bookinfo();
            try
            {
                // 图书信息查询语句
                string sqlforbookinfo = "select Top(1) Title, Decrible from BookInfo where BookID = @bookID";
                SqlParameter[] parameters = { new SqlParameter("@bookID", SqlDbType.UniqueIdentifier) };
                parameters[0].Value = bookID;
                bookinfo.BookID = bookID;
                using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqlforbookinfo, parameters))
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            // 图书名
                            if (!result.IsDBNull(0))
                            {
                                bookinfo.Title = result.GetString(0);
                            }

                            // 图书描述
                            if (!result.IsDBNull(1))
                            {
                                bookinfo.Decrible = result.GetString(1);
                            }
                        }
                    }

                    result.Close();
                }
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("根据图书ID查询图书信息", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return bookinfo;
        }

        /// <summary>
        /// 获取全部图书信息
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <returns>图书信息</returns>
        public List<Bookinfo> GetAllBookInfo(IDbConnection conn)
        {
            // 图书信息查询语句
            string sqlforbookinfo = "select Top(100) BookID, Title, Decrible from BookInfo";
            List<Bookinfo> bookinfoList = new List<Bookinfo>();
            try
            {
                using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqlforbookinfo, null))
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            Bookinfo bookinfo = new Bookinfo();

                            // 图书名
                            if (!result.IsDBNull(0))
                            {
                                bookinfo.BookID = result.GetGuid(0);
                            }

                            // 图书名
                            if (!result.IsDBNull(1))
                            {
                                bookinfo.Title = result.GetString(1);
                            }

                            // 图书描述
                            if (!result.IsDBNull(2))
                            {
                                bookinfo.Decrible = result.GetString(2);
                            }

                            bookinfoList.Add(bookinfo);
                        }
                    }

                    result.Close();
                }
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("获取全部图书信息", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return bookinfoList;
        }

        /// <summary>
        /// 模糊查询图书数
        /// </summary>
        /// <param name="bookname">查询关键字</param>
        /// <param name="conn">IDbConnection（数据库连接对象）</param>
        /// <returns>获得满足筛选条件的图书信息</returns>
        public List<Bookinfo> QueryBookByName(string bookname, IDbConnection conn)
        {
            string sqlText = "select top(100) BookID, Title, Decrible from BookInfo  WHERE Title LIKE '%" + bookname + "%'";
            List<Bookinfo> bookinfolist = new List<Bookinfo>();
            try
            {
                using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqlText, null))
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            Bookinfo stbookinfo = new Bookinfo();

                            // 图书ID
                            if (!result.IsDBNull(0))
                            {
                                stbookinfo.BookID = result.GetGuid(0);
                            }

                            // 图书名字
                            if (!result.IsDBNull(1))
                            {
                                stbookinfo.Title = result.GetString(1).Trim();
                            }

                            // 图书描述
                            if (!result.IsDBNull(2))
                            {
                                stbookinfo.Decrible = result.GetString(2).Trim();
                            }

                            bookinfolist.Add(stbookinfo);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("模糊查询图书数", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return bookinfolist;
        }

        /// <summary>
        /// 根据图书条形码查询借阅信息
        /// </summary>
        /// <param name="bookordernum">图书条形码</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>借阅信息</returns>
        public RrturnBookinfo QueryReturnBook(Guid bookordernum, IDbConnection conn)
        {
            //--借阅查询
            string sqltxt = @"select LoanInfo.UserName,LoanInfo.OrderNum,BookInfo.Title, LoanInfo.LoanDate,LoanInfo.IsPaied,LoanInfo.Fine 
            FROM LoanInfo INNER JOIN BookItem  ON LoanInfo.OrderNum = BookItem.OrderNum
            INNER JOIN BookInfo ON BookItem.BookID = BookInfo.BookID
            where BookItem.ISLoan = 1 and LoanInfo.OrderNum = @Ordernum";
            RrturnBookinfo rturnBookinfo = new RrturnBookinfo();

            SqlParameter[] parameters = { new SqlParameter("@Ordernum", SqlDbType.UniqueIdentifier) };
            parameters[0].Value = bookordernum;

            using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqltxt, parameters))
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

                        // 是否缴纳罚款
                        if (!result.IsDBNull(4))
                        {
                            rturnBookinfo.BookState = result.GetInt32(4);
                        }

                        // 罚款金额
                        if (!result.IsDBNull(5))
                        {
                            rturnBookinfo.PayMoney = result.GetDecimal(5);
                        }
                    }
                }
            }

            return rturnBookinfo;
        }

        /// <summary>
        /// 借阅查询
        /// </summary>
        /// <param name="username">借阅人</param>
        /// <param name="isdisreturn">是否查询已还图书</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>借阅信息</returns>
        public List<RrturnBookinfo> QueryBorrowBookInfo(string username, bool isdisreturn, IDbConnection conn)
        {
            //--借阅查询
            string sqltxt = @"select LoanInfo.UserName,LoanInfo.OrderNum,BookInfo.Title, LoanInfo.LoanDate, LoanInfo.IsPaied, LoanInfo.Fine FROM LoanInfo INNER JOIN BookItem  ON LoanInfo.OrderNum = BookItem.OrderNum INNER JOIN BookInfo ON BookItem.BookID = BookInfo.BookID where LoanInfo.UserName = @Username";
            StringBuilder sqlsb = new StringBuilder(sqltxt);
          
            // 如果要查询已还图书
            if (!isdisreturn)
            {
                sqlsb.Append("and BookItem.ISLoan = 1");
            }

            SqlParameter[] parameters = { new SqlParameter("@Username", SqlDbType.UniqueIdentifier) };
            parameters[0].Value = username;

            List<RrturnBookinfo> bookinfoList = new List<RrturnBookinfo>();
            using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqltxt, parameters))
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

                        // 是否缴纳罚款
                        if (!result.IsDBNull(5))
                        {
                            rturnBookinfo.BookState = result.GetInt32(5);
                        }

                        // 罚款金额
                        if (!result.IsDBNull(6))
                        {
                            rturnBookinfo.PayMoney = result.GetDecimal(6);
                        }

                        bookinfoList.Add(rturnBookinfo);
                    }
                }
            }

            return bookinfoList;
        }
    }
}
