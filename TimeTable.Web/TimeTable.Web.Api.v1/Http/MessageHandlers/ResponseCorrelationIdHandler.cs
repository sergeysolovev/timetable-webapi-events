using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.MessageHandlers
{
    /// <summary>
    /// Adds X-Correlation-Id guid to responses
    /// </summary>
    internal class ResponseCorrelationIdHandler : DelegatingHandler
    {
        private const string HeaderName = "X-Correlation-Id";

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var responseMessage = await base.SendAsync(request, cancellationToken);
            var correlationId = request.GetCorrelationId();

            responseMessage.Headers.Add(HeaderName, correlationId.ToString());

            return responseMessage;
        }
    }
}
