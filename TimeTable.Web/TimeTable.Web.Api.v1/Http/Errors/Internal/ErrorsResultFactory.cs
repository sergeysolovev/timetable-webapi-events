using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.Errors.Internal
{
    /// <summary>
    /// Local default for <see cref="IErrorsResultFactory"/>
    /// Based on content negotiation
    /// </summary>
    internal class ErrorsResultFactory : IErrorsResultFactory
    {
        public IHttpActionResult Create(
            HttpStatusCode statusCode,
            string message,
            ApiController controller,
            IEnumerable<string> otherErrors = null)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            var errorsContract = new ErrorsContract(statusCode, message, otherErrors)
            {
                HelpUrl = GetHelpUrl(controller.Url)
            };

            return new NegotiatedContentResult<ErrorsContract>(
                statusCode,
                errorsContract,
                controller
            );
        }

        public IHttpActionResult Create(
            HttpStatusCode statusCode,
            string message,
            HttpRequestMessage request,
            HttpConfiguration config,
            IEnumerable<string> otherErrors = null)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var formatters = config.Formatters;
            var contentNegotiator = config.Services.GetContentNegotiator();

            if (contentNegotiator == null)
            {
                throw new InvalidOperationException(
                    "Failed to resolve IContentNegotiator instance"
                );
            }

            var errorsContract = new ErrorsContract(statusCode, message, otherErrors)
            {
                HelpUrl = GetHelpUrl(request.GetUrlHelper())
            };

            return new NegotiatedContentResult<ErrorsContract>(
                statusCode,
                errorsContract,
                contentNegotiator,
                request,
                formatters
            );
        }

        private static string GetHelpUrl(UrlHelper url) => url?.Content($"~/{WebApi.HelpUrl}");
    }
}