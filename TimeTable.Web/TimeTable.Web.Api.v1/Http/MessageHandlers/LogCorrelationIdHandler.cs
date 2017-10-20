using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.MessageHandlers
{
    /// <summary>
    /// Adds request correlation id to <see cref="Trace.CorrelationManager"/>
    /// </summary>
    internal class LogCorrelationIdHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Trace.CorrelationManager.ActivityId = request.GetCorrelationId();
            return base.SendAsync(request, cancellationToken);
        }
    }
}
