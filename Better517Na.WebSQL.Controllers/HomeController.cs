//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: HomeController.cs
// * history : created by shaoyu 2014/1/16 8:41:32 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Better.Infrastructures.Log;
using Better517Na.LibrayMgr.Business;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Controllers
{
    /// <summary>
    /// HomeController
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 日志组件TrackID
        /// </summary>
        private TrackID trackID = TrackIdManager.GetInstance("shaoyu");

        /// <summary>
        /// 查询显示用户信息
        /// </summary>
        /// <param name="coll">传入参数</param>
        /// <returns>视图</returns>
        public ActionResult LibrayMgr(FormCollection coll)
        {
            this.trackID = TrackIdManager.GetInstance("shaoyu");
            List<DisplayInfo> displayInfolist = new List<DisplayInfo>();
            List<Bookinfo> bookinfoList = new List<Bookinfo>();
            try
            {
                // 初始信息
                if (coll.Count == 0)
                {
                    QueryAllBook queryAllBook = new QueryAllBook();
                    displayInfolist = queryAllBook.GetAllBookInfo();
                }
                else if (coll.Count > 0)
                {
                    // 查询信息
                    if (Request.IsAjaxRequest())
                    {
                        string bookname = coll["bookname"] != null ? coll["bookname"].Trim() : string.Empty;
                        DisplayMgr displayMgr = new DisplayMgr();

                        // 模糊查询
                        displayInfolist = displayMgr.QueryByName(bookname);
                        return this.PartialView("BookList", displayInfolist);
                    }
                }
            }
            catch (AppException appexp)
            {
                return this.View("Shared/Error", appexp.ToString());
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException(" 查询显示用户信息 ", exp, ExceptionLevel.Error);
                LogManager.Log.WriteException(appexp);
                return this.View("Shared/Error", exp.ToString());
            }
            finally
            {
                LogManager.Log.WriteUiAcc("shaoyu", "UserInfo", "shaoyu", "127.0.0.1", string.Empty, string.Empty, "查询显示用户信息", null);
            }

            return this.View(displayInfolist);
        }

        /// <summary>
        /// 借书页面
        /// </summary>
        /// <param name="coll">前台参数</param>
        /// <returns>借书 页面</returns>
        public ActionResult BorrowBook(FormCollection coll)
        {
            this.trackID = TrackIdManager.GetInstance("shaoyu");
            return this.View();
        }

        /// <summary>
        /// 借书页面Ajax 提交
        /// </summary>
        /// <param name="coll">参数</param>
        /// <returns>借书页面</returns>
        public string AjaxBorrowBook(FormCollection coll)
        {
            string result = string.Empty;
            try
            {
                string ispaied = coll["IsPaied"] != null ? coll["IsPaied"].Trim() : string.Empty;
                string orderNumguid = coll["bookid"] != null ? coll["bookid"].Trim() : string.Empty;
                string bookman = coll["name"] != null ? coll["name"].Trim() : string.Empty;
                Guid orderid = new Guid();
                if (Guid.TryParse(orderNumguid, out orderid))
                {
                    if (!string.IsNullOrEmpty(bookman))
                    {
                        switch (ispaied)
                        {
                            case "1":

                                // 付款并借书，付款失败不能借书
                                PayMoneyAndBorrowMgr payMoneyAndBorrowMgr = new PayMoneyAndBorrowMgr(bookman, orderid);
                                payMoneyAndBorrowMgr.Execute();
                                break;
                            case "0":
                                BorrowBookMgr borrowBookMgr = new BorrowBookMgr(bookman, orderid);
                                borrowBookMgr.Execute();
                                break;
                        }
                    }
                }
                else
                {
                    result = "图书条形码不符合规则";
                }
            }
            catch (AppException exp)
            {
                result = exp.Message;
            }
            catch (Exception exp)
            {
                result = exp.Message;
            }
            finally
            {
                LogManager.Log.WriteUiAcc("shaoyu", "AjaxBorrowBook", "shaoyu", "127.0.0.1", string.Empty, string.Empty, "借书页面Ajax 提交", null);
            }

            return result;
        }

        /// <summary>
        /// 借书 条形码提交后台获取书名
        /// </summary>
        /// <param name="coll">参数cool</param>
        /// <returns>书名</returns>
        public string GetBookName(FormCollection coll)
        {
            string result = string.Empty;
            try
            {
                if (coll.Count != 0)
                {
                    string orderNumguid = coll["bookid"] != null ? coll["bookid"].Trim() : string.Empty;
                    Guid orderguid = new Guid();
                    if (Guid.TryParse(orderNumguid, out orderguid))
                    {
                        GetBookItemInfoMgr getBookItemInfoMgr = new GetBookItemInfoMgr();
                        Guid bookid = getBookItemInfoMgr.GetBookID(orderguid);
                        Bookinfo bookinfo = new Bookinfo();
                        GetBookInfoMgr getBookInfoMgr = new GetBookInfoMgr();
                        bookinfo = getBookInfoMgr.GetBookInfoBybookid(bookid);
                        ViewBag.Bookname = bookinfo.Title;
                        result = bookinfo.Title;
                    }
                }
            }
            catch
            {
                result = string.Empty;
            }
            finally
            {
                LogManager.Log.WriteUiAcc("shaoyu", "UserInfo", "shaoyu", "127.0.0.1", string.Empty, string.Empty, "查询显示用户信息", null);
            }

            return result;
        }

        /// <summary>
        /// 检测借书人信息
        /// </summary>
        /// <param name="coll">参数cool</param>
        /// <returns>返回Check代码</returns>
        public string CheckBorrowName(FormCollection coll)
        {
            string message = string.Empty;
            try
            {
                if (coll.Count != 0)
                {
                    string bookman = coll["name"] != null ? coll["name"].Trim() : string.Empty;
                    if (!string.IsNullOrEmpty(bookman))
                    {
                        CheckUserInfoMgr checkUserInfo = new CheckUserInfoMgr(bookman);
                        checkUserInfo.Execute();
                        message = checkUserInfo.Checkuser(bookman);
                    }
                }
            }
            catch
            {
                message = "借书人信息检测未通过";
            }
            finally
            {
                LogManager.Log.WriteUiAcc("shaoyu", "UserInfo", "shaoyu", "127.0.0.1", string.Empty, string.Empty, "查询显示用户信息", null);
            }

            return message;
        }

        /// <summary>
        /// 还书页面控制器
        /// </summary>
        /// <param name="coll">参数</param>
        /// <returns>页面</returns>
        public ActionResult ReturnBook(FormCollection coll)
        {
            return this.View();
        }

        /// <summary>
        /// 还书信息检测AJAX处理器
        /// </summary>
        /// <param name="coll">参数</param>
        /// <returns>检测结果</returns>
        public string AjaxCheckReturnBook(FormCollection coll)
        {
            string result = string.Empty;
            try
            {
                string orderNumguid = coll["bookguid"] != null ? coll["bookguid"].Trim() : string.Empty;
                Guid orderid = new Guid();
                if (Guid.TryParse(orderNumguid, out orderid))
                {
                    GetLoanInfoMgr getLoanInfoMgr = new GetLoanInfoMgr();
                    OutTimeBookInfo outTimeBookInfo = new OutTimeBookInfo();
                    outTimeBookInfo = getLoanInfoMgr.GetReturnBookInfo(orderid);
                    DisplayMgr displayMgr = new DisplayMgr();
                    if (!string.IsNullOrEmpty(outTimeBookInfo.UserName))
                    {
                        result = displayMgr.FitMessage(outTimeBookInfo);
                    }
                    else
                    {
                        result = "该书已经归还";
                    }
                }
                else
                {
                    result = "图书条形码不符合规则";
                }
            }
            catch (AppException exp)
            {
                result = exp.Message;
            }
            catch (Exception exp)
            {
                result = exp.Message;
            }
            finally
            {
                LogManager.Log.WriteUiAcc("shaoyu", "AjaxBorrowBook", "shaoyu", "127.0.0.1", string.Empty, string.Empty, "借书页面Ajax 提交", null);
            }

            return result;
        }

        /// <summary>
        /// 还书AJAX处理器
        /// </summary>
        /// <param name="coll">参数</param>
        /// <returns>处理结果</returns>
        public string AjaxReturnBook(FormCollection coll)
        {
            string result = string.Empty;
            try
            {
                string flag = coll["Returnflag"] != null ? coll["Returnflag"].Trim() : string.Empty;
                string bookguid = coll["bookid"] != null ? coll["bookid"].Trim() : string.Empty;
                Guid orderid = new Guid();
                if (!string.IsNullOrEmpty(flag) && Guid.TryParse(bookguid, out orderid))
                {
                    ReturnBookCtrMgr returnBookCtrMgr = new ReturnBookCtrMgr();
                    returnBookCtrMgr.RetrunBook(flag, orderid);
                }
            }
            catch (AppException exp)
            {
                result = exp.Message;
            }
            catch (Exception exp)
            {
                result = exp.Message;
            }
            finally
            {
                LogManager.Log.WriteUiAcc("shaoyu", "AjaxBorrowBook", "shaoyu", "127.0.0.1", string.Empty, string.Empty, "借书页面Ajax 提交", null);
            }

            return result;
        }

        /// <summary>
        /// 借阅查询
        /// </summary>
        /// <param name="coll">参数</param>
        /// <returns>页面</returns>
        public ActionResult BorrowBookQuery(FormCollection coll)
        {
            return this.View();
        }

        /// <summary>
        /// 借阅查询处理器
        /// </summary>
        /// <param name="coll">参数</param>
        /// <returns>处理结果</returns>
        public ActionResult AjaxBorrowBookQuery(FormCollection coll)
        {
            string result = string.Empty;
            List<RrturnBookinfo> rturnBookinfoList = new List<RrturnBookinfo>();
            try
            {
                string flag = coll["flag"] != null ? coll["flag"].Trim() : string.Empty;
                string username = coll["username"] != null ? coll["username"].Trim() : string.Empty;
                if (!string.IsNullOrEmpty(flag))
                {
                    DisplayMgr displamgr = new DisplayMgr();
                    rturnBookinfoList = displamgr.FitBorrowInfo(flag == "1", username);
                }
            }
            catch (AppException exp)
            {
                result = exp.Message;
            }
            catch (Exception exp)
            {
                result = exp.Message;
            }
            finally
            {
                LogManager.Log.WriteUiAcc("shaoyu", "AjaxBorrowBook", "shaoyu", "127.0.0.1", string.Empty, string.Empty, "借书页面Ajax 提交", null);
            }

            return this.PartialView("BorrowList", rturnBookinfoList);
        }
    }
}
