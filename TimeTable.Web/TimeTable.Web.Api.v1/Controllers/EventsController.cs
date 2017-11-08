using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Docs;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpbuEducation.TimeTable.Web.Api.v1.Controllers
{
    [RoutePrefix(WebApi.RoutePrefix)]
    class EventsController : ApiController
    {
        private readonly IErrorsResultFactory errorsFactory;

        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="errorsFactory"></param>
        public EventsController(IErrorsResultFactory errorsFactory)
        {
            this.errorsFactory = errorsFactory ?? 
                throw new ArgumentNullException(nameof(errorsFactory));
        }

        [HttpGet]
        [Route("groups/{id}/events/{from}/{to}")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetEvents(string id, string from, string to,
            TimeTableKindСode timetable = TimeTableKindСode.Unknown)
        {
            if (!int.TryParse(id, out int idValue))
            {
                return errorsFactory.CreateBadRequest(this, id, nameof(id));
            }

            if (!DateTime.TryParse(from, out DateTime fromValue))
            {
                return errorsFactory.CreateBadRequest(this, from, nameof(from));
            }

            if (!DateTime.TryParse(to, out DateTime toValue))
            {
                return errorsFactory.CreateBadRequest(this, to, nameof(to));
            }
            return Content(HttpStatusCode.OK,"test");
        }
    }
}
