//-----------------------------------------------------------------------
// <copyright file="SharedController.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: SharedController.cs
// * history : created by shaoyu 2014/1/16 20:42:46 
// </copyright>
//-----------------------------------------------------------------------
using System.Web.Mvc;

namespace Better517Na.LibrayMgr.Controllers
{
    /// <summary>
    /// SharedController
    /// </summary>
    public class SharedController : Controller
    {
        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="erros">错误信息</param>
        /// <returns>错误页面</returns>
        public ActionResult Error(string erros)
        {
            return this.View(erros);
        }
    }
}
