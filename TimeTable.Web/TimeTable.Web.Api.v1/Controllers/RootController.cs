using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using System;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace SpbuEducation.TimeTable.Web.Api.v1.Controllers
{
    /// <summary>
    /// Root controller
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [RoutePrefix(WebApi.RoutePrefix)]
    public class RootController : ApiController
    {
        private readonly ILogger logger;
        private readonly IErrorsResultFactory errorsFactory;

        /// <summary>
        /// Creates root controller
        /// </summary>
        public RootController()
        {
        }

        /// <summary>
        /// Handles api root endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            string prefix(string endpointUrl) =>
                Url.Content($"~/") + $"{WebApi.RoutePrefix}/{endpointUrl}";

            var endpoints = new
            {
                addressesUrl = prefix("addresses"),
                classroomsUrl = prefix("addresses/{oid}/classrooms"),
                educatorsSearchUrl = prefix("educators/search/{query}"),
                studyDivisionsUrl = prefix("study/divisions"),
                extracurricularDivisions = prefix("extracur/divisions"),
                groupsUrls = prefix("programs/{id}/groups"),
                programsLevelsUrls = prefix("study/divisions/{alias}/programs/levels"),
                groupEventsUrl = prefix("groups/{id}/events{/from}"),
                educatorEventsUrl = prefix("educators/{id}/events"),
                extracurricularEventsUrl = prefix("extracur/divisions/{alias}/events?fromDate={fromDate}")
            };

            return Json(endpoints, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
        }
    }
}
