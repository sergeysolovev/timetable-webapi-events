using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Threading;
using NLog;
using SpbuEducation.TimeTable.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.Controllers
{
    public class BaseController: Controller
    {
        private static readonly Logger logger = LogManager.GetLogger("Web");

        /// <summary>
        /// Устанавливает cookie, в котором содержится идентификатор культуры (языка),
        /// полученный от клиента
        /// </summary>
        /// <param name="clientCultureName"></param>
        /// <returns></returns>
        public ActionResult SetClientCultureCookie(string clientCultureName)
        {
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
            {
                cookie.Value = clientCultureName;
            }
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = clientCultureName;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);

            return Redirect(Request.UrlReferrer.ToString());
        }                

        /// <summary>
        /// Вызывается для каждого запроса, идущего через любой контроллер,
        /// наследующий BaseController. Устанавливает текущую культуру для запроса
        /// на основе информации от клиента
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string clientCultureName = GetClientCultureName();
            CultureHelper.SetCurrentCulture(clientCultureName);
            return base.BeginExecuteCore(callback, state);
        }

        /// <summary>
        /// Получает информацию о культуре от клиента через Cookie или
        /// HTTP_ACCEPT_LANGUAGE
        /// </summary>
        /// <returns></returns>
        private string GetClientCultureName()
        {
            string clientCultureName = null;
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                clientCultureName = cultureCookie.Value;
            }
            else
            {
                clientCultureName = Request.UserLanguages != null && Request.UserLanguages.Any() ? Request.UserLanguages.First() : null;
            }
            return clientCultureName;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // here is the usefull method
            var eventInfo = new LogEventInfo(LogLevel.Error, logger.Name, filterContext.Exception.Message)
            {
                Exception = filterContext.Exception
            };

            eventInfo.Properties["Controller"] = filterContext.RouteData.Values["controller"];
            eventInfo.Properties["Action"] = filterContext.RouteData.Values["action"];
            eventInfo.Properties["View"] = eventInfo.Properties["Action"];
            eventInfo.Properties["Url"] = HttpContext.Request.Url != null ? HttpContext.Request.Url.AbsoluteUri : null;
            // TODO: Add HttpCode and UserIP to inventInfo
            //var httpException = filterContext.Exception as HttpException;
            //eventInfo.Properties["HttpCode"] = httpException?.GetHttpCode();
            //eventInfo.Properties["UserIP"] = HttpContext.Request.UserHostAddress;

            logger.Log(eventInfo);

            base.OnException(filterContext);
        }
    }
}