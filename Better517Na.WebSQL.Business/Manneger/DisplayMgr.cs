//-----------------------------------------------------------------------
// <copyright file="DisplayMgr.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: DisplayMgr.cs
// * history : created by shaoyu 2014/1/17 14:43:02 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Better517Na.LibrayMgr.Models;

namespace Better517Na.LibrayMgr.Business
{
    /// <summary>
    /// DisplayMgr
    /// </summary>
    public class DisplayMgr
    {
        /// <summary>
        /// 组装显示信息
        /// </summary>
        /// <returns>显示信息</returns>
        public List<DisplayInfo> GetDisplayinfo()
        {
            GetBookInfoMgr getBookInfoMgr = new Business.GetBookInfoMgr();
            GetBookItemInfoMgr getBookItemInfoMgr = new GetBookItemInfoMgr();
            List<DisplayInfo> displayInfolist = new List<DisplayInfo>();
            List<Bookinfo> bookinfoList = new List<Bookinfo>();
           
            // 获取全部图书信息
            bookinfoList = getBookInfoMgr.GetAllBookInfo();
            foreach (Bookinfo tempinfo in bookinfoList)
            {
                DisplayInfo displayInfo = new DisplayInfo();
                displayInfo.BookID = tempinfo.BookID;
                displayInfo.Title = tempinfo.Title;
                displayInfo.Decrible = tempinfo.Decrible;

                // 获取该书的总数
                displayInfo.BookNum = getBookItemInfoMgr.GetKindBookSum(tempinfo.BookID);

                // 获取该数的可借阅数
                displayInfo.Bookleft = getBookItemInfoMgr.GetISloanBookSum(tempinfo.BookID);
                displayInfolist.Add(displayInfo);
            }

            return displayInfolist;
        }

        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="bookname">借阅人</param>
        /// <returns>图书</returns>
        public List<DisplayInfo> QueryByName(string bookname)
        {
            GetBookInfoMgr getBookInfoMgr = new GetBookInfoMgr();
            GetBookItemInfoMgr getBookItemInfoMgr = new GetBookItemInfoMgr();
            List<DisplayInfo> displayInfolist = new List<DisplayInfo>();
            List<Bookinfo> bookinfoList = new List<Bookinfo>();

            // 获取全部图书信息
            bookinfoList = getBookInfoMgr.QueryBookByName(bookname);

            foreach (Bookinfo tempinfo in bookinfoList)
            {
                DisplayInfo displayInfo = new DisplayInfo();
                displayInfo.BookID = tempinfo.BookID;
                displayInfo.Title = tempinfo.Title;
                displayInfo.Decrible = tempinfo.Decrible;

                // 获取该书的总数
                displayInfo.BookNum = getBookItemInfoMgr.GetKindBookSum(tempinfo.BookID);

                // 获取该数的可借阅数
                displayInfo.Bookleft = getBookItemInfoMgr.GetISloanBookSum(tempinfo.BookID);
                displayInfolist.Add(displayInfo);
            }

            return displayInfolist;
        }

        /// <summary>
        /// 查询并组装返回未还书的超期信息
        /// </summary>
        /// <param name="outTimeBookInfo">信息来源</param>
        /// <returns>组装好的提示信息</returns>
        public string FitMessage(OutTimeBookInfo outTimeBookInfo)
        {
            StringBuilder messb = new StringBuilder();
            messb.Append(outTimeBookInfo.UserName + "#");
            messb.Append(outTimeBookInfo.Booktitle);
            if (outTimeBookInfo.BorrowDays > 30)
            {
                messb.Append("#每本书最多可借阅30天，你已借阅");
                messb.Append(outTimeBookInfo.BorrowDays + "天,超期" + (outTimeBookInfo.BorrowDays - 30).ToString() + "天");
            }

            if (outTimeBookInfo.Money > 0)
            {
                messb.Append("请缴纳超期费用！超期费用为" + outTimeBookInfo.Money + "元");
            }

            return messb.ToString();
        }

        /// <summary>
        /// 获取借阅信息
        /// </summary>
        /// <param name="isback">是否已还 true 已还，false 未还</param>
        /// <param name="username">用户id</param>
        /// <returns>借阅信息表</returns>
        public List<RrturnBookinfo> FitBorrowInfo(bool isback, string username)
        {
            QueryBorrowBookMgr queryBorrowBookMgr = new QueryBorrowBookMgr(isback, username);
            queryBorrowBookMgr.Execute();
            List<RrturnBookinfo> rturnBookinfoList = queryBorrowBookMgr.RturnBookinfoList;
            if (isback)
            {
                // 已还
                foreach (RrturnBookinfo tempRrturnBookinfo in rturnBookinfoList)
                {
                    if (tempRrturnBookinfo.BookState == 1)
                    {
                        tempRrturnBookinfo.BookPayStat = "缴纳";
                    }
                    else
                    {
                        tempRrturnBookinfo.BookPayStat = "未缴纳";
                    }
                }
            }

            return rturnBookinfoList;
        }
    }
}
