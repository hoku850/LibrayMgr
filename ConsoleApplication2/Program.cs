using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Better517Na.LibrayMgr.Business;
using Better517Na.LibrayMgr.Models;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {

            // 全部查询
            DisplayMgr DisplayMgr = new DisplayMgr();
            List<DisplayInfo> displaylist1 = new List<DisplayInfo>();
            displaylist1 = DisplayMgr.GetDisplayinfo();

            // 模糊查询

            List<DisplayInfo> displaylist2 = new List<DisplayInfo>();
            displaylist2 = DisplayMgr.QueryByName("英语");


            // 借书
            Guid orderid = new Guid("B24B5470-755E-4EB5-8AE8-00116B0355F6");
            string name = "张三";
            BorrowBookMgr BorrowBookMgr = new BorrowBookMgr(name, orderid);
            BorrowBookMgr.Execute();
            Console.ReadLine();
        }
    }
}
