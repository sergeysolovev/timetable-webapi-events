using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.Errors
{
    /// <summary>
    /// Extension methods for <see cref="IErrorsResultFactory"/>
    /// </summary>
    public static class ErrorsResultFactoryExtensions
    {
        /// <summary>
        /// Creates BadRequest (400) custom error result
        /// using controller context
        /// </summary>
        /// <param name="source"></param>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        /// <param name="otherErrors"></param>
        /// <returns></returns>
        public static IHttpActionResult CreateBadRequest(
            this IErrorsResultFactory source,
            ApiController controller,
            string message,
            IEnumerable<string> otherErrors = null)
        {
            return source.Create(HttpStatusCode.BadRequest, message, controller, otherErrors);
        }

        /// <summary>
        /// Creates BadRequest (400) custom error result
        /// using controller context
        /// </summary>
        /// <param name="source"></param>
        /// <param name="controller"></param>
        /// <param name="paramValue"></param>
        /// <param name="paramName"></param>
        /// <param name="otherErrors"></param>
        /// <returns></returns>
        public static IHttpActionResult CreateBadRequest(
            this IErrorsResultFactory source,
            ApiController controller,
            object paramValue,
            string paramName,
            IEnumerable<string> otherErrors = null)
        {
            var message = $"Parameter {paramName}: '{paramValue}' is not valid";
            return source.Create(HttpStatusCode.BadRequest, message, controller, otherErrors);
        }

        /// <summary>
        /// Creates BadRequest (400) custom error result
        /// using request context
        /// </summary>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <param name="config"></param>
        /// <param name="message"></param>
        /// <param name="otherErrors"></param>
        /// <returns></returns>
        public static IHttpActionResult CreateBadRequest(
            this IErrorsResultFactory source,
            HttpRequestMessage request,
            HttpConfiguration config,
            string message,
            IEnumerable<string> otherErrors = null)
        {
            return source.Create(HttpStatusCode.BadRequest, message, request, config, otherErrors);
        }

        /// <summary>
        /// Creates NotFound (404) custom error result
        /// using controller context
        /// </summary>
        /// <param name="source"></param>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        /// <param name="otherErrors"></param>
        /// <returns></returns>
        public static IHttpActionResult CreateNotFound(
            this IErrorsResultFactory source,
            ApiController controller,
            string message,
            IEnumerable<string> otherErrors = null)
        {
            return source.Create(HttpStatusCode.NotFound, message, controller, otherErrors);
        }

        /// <summary>
        /// Creates NotFound (404) custom error result
        /// using request context
        /// </summary>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <param name="config"></param>
        /// <param name="message"></param>
        /// <param name="otherErrors"></param>
        /// <returns></returns>
        public static IHttpActionResult CreateNotFound(
            this IErrorsResultFactory source,
            HttpRequestMessage request,
            HttpConfiguration config,
            string message,
            IEnumerable<string> otherErrors = null)
        {
            return source.Create(HttpStatusCode.NotFound, message, request, config, otherErrors);
        }

        /// <summary>
        /// Creates InternalServerError (500) custom error result
        /// using controller context
        /// </summary>
        /// <param name="source"></param>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        /// <param name="otherErrors"></param>
        /// <returns></returns>
        public static IHttpActionResult CreateInternalServerError(
            this IErrorsResultFactory source,
            ApiController controller,
            string message,
            IEnumerable<string> otherErrors = null)
        {
            return source.Create(HttpStatusCode.InternalServerError, message, controller, otherErrors);
        }

        /// <summary>
        /// Creates InternalServerError (500) custom error result
        /// using request context
        /// </summary>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <param name="config"></param>
        /// <param name="message"></param>
        /// <param name="otherErrors"></param>
        /// <returns></returns>
        public static IHttpActionResult CreateInternalServerError(
            this IErrorsResultFactory source,
            HttpRequestMessage request,
            HttpConfiguration config,
            string message,
            IEnumerable<string> otherErrors = null)
        {
            return source.Create(HttpStatusCode.InternalServerError, message, request, config, otherErrors);
        }
    }
}
