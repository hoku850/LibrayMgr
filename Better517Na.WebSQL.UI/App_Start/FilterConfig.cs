using System.Web;
using System.Web.Mvc;

namespace Better517Na.LibrayMgr.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}