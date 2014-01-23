using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Better517Na.WebSQL.Models;
using Better517Na.WebSQL.Business;
using Better517Na.WebSQL.Services;
using Better.Infrastructures.DBUtility;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("rows -->影响数据的行数，-1 表示操作员编号已经存在 ");
            Console.WriteLine();
            StaffInfo newstaffinfo = new StaffInfo();
            newstaffinfo.StaffID = "3";
            newstaffinfo.StaffName = "王大锤";
            newstaffinfo.BelongDeptId = 2;
            DeptInfo newdeptinfo = new DeptInfo();
            newdeptinfo.DeptID = 1;
            newdeptinfo.DeptName = "测试部";
            newdeptinfo.DeptPhone = "12312312";

            AddStaffMgr AddStaffmger = new AddStaffMgr(newstaffinfo);
            AddStaffmger.Execute();

            StaffQueryMgr StaffQuery = new StaffQueryMgr(newstaffinfo.StaffID);
            StaffQuery.Execute();
            List<StaffDeptInfo> staffDeptinfoList = StaffQuery.StaffDeptinfoList;

            DeleteStaffMgr DeleteStaff = new DeleteStaffMgr(newstaffinfo.StaffID);
            DeleteStaff.Execute();

            UpdataStaffMgr UpdataStaff = new UpdataStaffMgr(newstaffinfo);

            UpdataStaff.Execute();

            Console.ReadLine();
        }
    }
}
