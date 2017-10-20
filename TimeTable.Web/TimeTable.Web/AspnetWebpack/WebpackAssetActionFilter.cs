using System;
using System.Web;
using System.Web.Mvc;

namespace SpbuEducation.TimeTable.Web.AspnetWebpack
{
    internal class WebpackAssetActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // set webpack assets urls to controller's ViewBag
            // to link them from the views
            WebpackAssetsUrls assetsUrls = null;
            try
            {
                var viewBag = filterContext.Controller.ViewBag;

                // TODO: use non-blocking async/await after update to aspnet.core
                // TODO: pass continueOnCapturedContext = true in async context
                // TODO: no need to catch AggregateException in async/await code
                assetsUrls = WebpackHelper.GetAssetsUrlsAsync(false).Result;
                viewBag.Assets = assetsUrls;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }

            // redirect asset requests (e.g. dynamic chunks loading)
            // to webpack dev server upstream
            if (WebpackHelper.UseDevServer)
            {
                var request = filterContext.HttpContext.Request;
                var assetPath = request.AppRelativeCurrentExecutionFilePath;
                var assetName = VirtualPathUtility.GetFileName(assetPath);
                var assetUrl = assetsUrls.Get(assetName);
                if (!string.IsNullOrEmpty(assetUrl))
                {
                    filterContext.Result = new RedirectResult(assetUrl);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}