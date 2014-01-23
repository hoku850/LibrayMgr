//-----------------------------------------------------------------------
// <copyright file="QueryAllBook.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: QueryAllBook.cs
// * history : created by shaoyu 2014/1/18 15:56:50 
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
    /// 获取所有图书信息；类
    /// </summary>
    public class QueryAllBook
    {
        /// <summary>
        /// 获取所有图书信息
        /// </summary>
        /// <returns>所有图书信息</returns>
        public List<DisplayInfo> GetAllBookInfo()
        {
            List<DisplayInfo> displayInfolist = new List<DisplayInfo>();
            List<Bookinfo> bookinfoList = new List<Bookinfo>();
            
            // 全部显示
            GetBookInfoMgr getBookInfoMgr = new GetBookInfoMgr();
            GetBookItemInfoMgr getBookItemInfoMgr = new GetBookItemInfoMgr();

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
    }
}
