using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.Errors
{
    /// <summary>
    /// Creates custom error http result
    /// </summary>
    public interface IErrorsResultFactory
    {
        /// <summary>
        /// Creates <see cref="IHttpActionResult"/>
        /// using controller context
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="controller"></param>
        /// <param name="otherErrors"></param>
        /// <returns></returns>
        IHttpActionResult Create(
            HttpStatusCode statusCode,
            string message,
            ApiController controller,
            IEnumerable<string> otherErrors = null
        );

        /// <summary>
        /// Creates <see cref="IHttpActionResult"/>
        /// using request context
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="request"></param>
        /// <param name="config"></param>
        /// <param name="otherErrors"></param>
        /// <returns></returns>
        IHttpActionResult Create(
            HttpStatusCode statusCode,
            string message,
            HttpRequestMessage request,
            HttpConfiguration config,
            IEnumerable<string> otherErrors = null
        );
    }
}
