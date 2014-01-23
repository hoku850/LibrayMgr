//-----------------------------------------------------------------------
// <copyright file="BookItemAccess.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: BookItemAccess.cs
// * history : created by shaoyu 2014/1/16 9:26:37 
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
    /// DStaffAccess
    /// </summary>
    public class BookItemAccess : IBookItemAccess
    {
        /// <summary>
        /// 查询总的图书藏书与可借书信息
        /// </summary>
        /// <param name="conn">IDbConnection数据库连接对象</param> 
        /// <returns>总的图书藏书与可借书信息</returns>
        public List<DisplayInfo> QuerySumISLoan(IDbConnection conn)
        {
            // 用来显示
            List<DisplayInfo> displayInfolist = new List<DisplayInfo>();

            // 统计数量查询语句
            string sqlforbookitem = "select OrderNum, BookID, ISLoan from BookItem";
            try
            {
                // 统计数量
                using (SqlDataReader result = SqlHelper.ExecuteReader(null, conn as SqlConnection, sqlforbookitem, null))
                {
                    if (result.HasRows)
                    {
                        // 统计数量
                        List<BookItem> booknum = new List<BookItem>();
                        while (result.Read())
                        {
                            BookItem tempBookItem = new BookItem();
                            
                            // 条形码
                            if (!result.IsDBNull(0))
                            {
                                tempBookItem.OrderNum = result.GetGuid(0);
                            }

                            // 图书ID
                            if (!result.IsDBNull(1))
                            {
                                tempBookItem.BookID = result.GetGuid(1);
                            }

                            // 图书是否可借
                            if (!result.IsDBNull(2))
                            {
                                tempBookItem.ISLoan = result.GetInt32(2);
                            }

                            booknum.Add(tempBookItem);
                        }

                        displayInfolist = this.GetRepeat(booknum);
                    }
                }
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("查询总的图书藏书与可借书信息", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return displayInfolist;
        }

        /// <summary>
        /// 根据图书ID查询图书总数
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>该书总数</returns>
        public int GetKindBookSum(Guid bookID, IDbConnection conn)
        {
            object obj = null;
            try
            {
                string sqlforbookitem = "SELECT COUNT(BookID)  FROM BookItem WHERE BookID= @bookID";
                SqlParameter[] parameters = { new SqlParameter("@bookID", SqlDbType.UniqueIdentifier) };
                parameters[0].Value = bookID;
                obj = SqlHelper.ExecuteScalar(null, conn as SqlConnection, sqlforbookitem, parameters);
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("根据图书ID查询图书总数", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return (int)obj;
        }

        /// <summary>
        /// 根据图书ID查询图书可借数
        /// </summary>
        /// <param name="bookID">图书ID</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>可借数</returns>
        public int GetISloanBookSum(Guid bookID, IDbConnection conn)
        {
            object obj = null;

            // 统计数量查询语句
            try
            {
                string sqlforbookitem = "SELECT COUNT(BookID)  FROM BookItem WHERE (ISLoan = 0 and BookID= @bookID)";
                SqlParameter[] parameters = { new SqlParameter("@bookID", SqlDbType.UniqueIdentifier) };
                parameters[0].Value = bookID;
                obj = SqlHelper.ExecuteScalar(null, conn as SqlConnection, sqlforbookitem, parameters);
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("根据图书ID查询图书可借数", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return (int)obj;
        }

        /// <summary>
        /// 图书借阅
        /// </summary>
        /// <param name="ordernum">图书条形码</param>
        /// <param name="trans">事务</param>
        /// <returns>成功返回true 失败false</returns>
        public bool BorrowBook(Guid ordernum, IDbTransaction trans)
        {
            bool isok = false;
            try
            {
                string sqltext = " UPDATE  Top (1) BookItem SET ISLoan = 1 WHERE OrderNum = @ordernum and ISLoan = 0";
                SqlParameter[] parameters = { new SqlParameter("@ordernum", SqlDbType.UniqueIdentifier) };
                parameters[0].Value = ordernum;
                int result = SqlHelper.ExecuteSql(trans as SqlTransaction, null, sqltext, parameters);
                if (result == 1)
                {
                    isok = true;
                }
                else
                {
                    isok = false;
                }
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("图书借阅", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return isok;
        }

        /// <summary>
        /// 还书,图书入库
        /// </summary>
        /// <param name="ordernum">图书条形码</param>
        /// <param name="trans">事务</param>
        /// <returns>返回影响的行数</returns>
        public bool ReturnBook(Guid ordernum, IDbTransaction trans)
        {
            bool isok = false;
            try
            {
                string sqltext = " UPDATE  Top (1) BookItem SET ISLoan = 0 WHERE OrderNum = @ordernum and ISLoan = 1";
                SqlParameter[] parameters = { new SqlParameter("@ordernum", SqlDbType.UniqueIdentifier) };
                parameters[0].Value = ordernum;
                int result = SqlHelper.ExecuteSql(trans as SqlTransaction, null, sqltext, parameters);
                if (result == 1)
                {
                    isok = true;
                }
                else
                {
                    isok = false;
                }
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("图书借阅", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return isok;
        }

        /// <summary>
        /// 根据图书条形码查ID
        /// </summary>
        /// <param name="ordernum">图书条形码</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>图书ID</returns>
        public Guid GetBookID(Guid ordernum, IDbConnection conn)
        {
            object obj = null;
            try
            {
                string sqlforbookitem = "SELECT BookID  FROM BookItem WHERE OrderNum = @ordernum";
                SqlParameter[] parameters = { new SqlParameter("@ordernum", SqlDbType.UniqueIdentifier) };
                parameters[0].Value = ordernum;
                obj = SqlHelper.ExecuteScalar(null, conn as SqlConnection, sqlforbookitem, parameters);
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("根据图书条形码查ID", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
            }

            return (Guid)obj;
        }

        /// <summary>
        /// 统计图书数量和可借阅数
        /// </summary>
        /// <param name="booknum">图书信息</param>
        /// <returns>显示数据</returns>
        private List<DisplayInfo> GetRepeat(List<BookItem> booknum)
        {
            List<DisplayInfo> displayInfolist = new List<DisplayInfo>();
            for (int i = 0; i < booknum.Count; i++)
            {
                int sum = 0;
                int isloan = 0;
                DisplayInfo tempdisplayInfo = new DisplayInfo();
                tempdisplayInfo.BookID = booknum[i].BookID;
                for (int j = 0; j < booknum.Count; j++)
                {
                    if (booknum[i].BookID == booknum[j].BookID)
                    {
                        // 记录重复数
                        sum++;
                        if (booknum[j].ISLoan == 0)
                        {
                            // 记录图书可借阅数
                            isloan++;
                        }

                        // 从列表中删除重复数据
                        booknum.Remove(booknum[j]);
                    }
                }

                tempdisplayInfo.BookNum = sum;
                tempdisplayInfo.Bookleft = isloan;
                displayInfolist.Add(tempdisplayInfo);
            }

            return displayInfolist;
        }
    }
}
