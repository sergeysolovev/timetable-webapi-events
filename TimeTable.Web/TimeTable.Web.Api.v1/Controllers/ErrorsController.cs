using NLog;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace SpbuEducation.TimeTable.Web.Api.v1.Controllers
{
    /// <summary>
    /// Errors controller
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [RoutePrefix(WebApi.RoutePrefix)]
    public class ErrorsController : ApiController
    {
        private readonly ILogger logger;
        private readonly IErrorsResultFactory errorsFactory;

        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="errorsFactory"></param>
        public ErrorsController(
            ILogger logger,
            IErrorsResultFactory errorsFactory)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            this.errorsFactory = errorsFactory ??
                throw new ArgumentNullException(nameof(errorsFactory));
        }

        /// <summary>
        /// For catching and handling the routes that aren't matching
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions]
        public IHttpActionResult NotFound(string path)
        {
            var apiPath = Request.RequestUri.AbsolutePath;
            var message = $"The path was not found: {apiPath}";

            logger.Error(new HttpException(404, message), message);

            return errorsFactory.CreateNotFound(this, message);
        }
    }
}
