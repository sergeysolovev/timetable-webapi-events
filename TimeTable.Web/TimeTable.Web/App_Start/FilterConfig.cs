using SpbuEducation.TimeTable.Web.AspnetWebpack;
using SpbuEducation.TimeTable.Web.Helpers;
using System.Web;
using System.Web.Mvc;

namespace SpbuEducation.TimeTable.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new WebpackAssetActionFilter());
        }
    }
}