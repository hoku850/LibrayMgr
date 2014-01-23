//-----------------------------------------------------------------------
// <copyright file="ReturnBookCtrMgr.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: ReturnBookCtrMgr.cs
// * history : created by shaoyu 2014/1/20 14:31:38 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using Better.Infrastructures.Log;
using Better517Na.LibrayMgr.Factory;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    /// 综合还书管理器
    /// </summary>
    public class ReturnBookCtrMgr
    {
        /// <summary>
        /// 综合还书
        /// </summary>
        /// <param name="flag">还书类型</param>
        /// <param name="bookid">图书条形码</param>
        /// <returns>返回还书信息</returns>
        public string RetrunBook(string flag, Guid bookid)
        {
            string reslut = string.Empty;
            try
            {
                OutTimeBookInfo outTimeBookInfo = new OutTimeBookInfo();
                GetLoanInfoMgr getLoanInfoMgr = new GetLoanInfoMgr();

                // 获取未还图书信息
                outTimeBookInfo = getLoanInfoMgr.GetReturnBookInfo(bookid);
                switch (flag)
                {
                    // 正常还书无罚款
                    case "0":
                        ReturnbookNomle returnbookNomMgr = new ReturnbookNomle(bookid);
                        returnbookNomMgr.Execute();
                        break;

                    // 立即缴纳并归还图书
                    case "1":
                        ReturnBookPayMoney returnBookPayMoney = new ReturnBookPayMoney(bookid, (decimal)outTimeBookInfo.Money);
                        returnBookPayMoney.Execute();
                        break;

                    // 还书但暂时不缴纳罚款
                    case "2":
                        ReturnOutTimeBookNoPay returnOutTimeBookNoPay = new ReturnOutTimeBookNoPay(bookid, (decimal)outTimeBookInfo.Money);
                        returnOutTimeBookNoPay.Execute();
                        break;
                }
            }
            catch (AppException exp)
            {
                reslut = exp.Message;
            }
            catch (Exception exp)
            {
                AppException appexp = new AppException("还书失败", exp, ExceptionLevel.Info);
                throw appexp;
            }

            return reslut;
        }
    }
}
