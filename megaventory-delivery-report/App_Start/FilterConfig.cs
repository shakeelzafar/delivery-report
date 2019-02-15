using System.Web;
using System.Web.Mvc;

namespace megaventory_delivery_report
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}