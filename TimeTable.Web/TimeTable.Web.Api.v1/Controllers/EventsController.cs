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
    /// <summary>
    /// Events Controller
    /// </summary>
    [RoutePrefix(WebApi.RoutePrefix)]
    public class EventsController : ApiController
    {
        private readonly IErrorsResultFactory errorsFactory;
        private readonly IEventsService eventsService;
        
        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="errorsFactory"></param>
        public EventsController(
            IErrorsResultFactory errorsFactory,
            IEventsService eventsService)
        {
            this.errorsFactory = errorsFactory ?? 
                throw new ArgumentNullException(nameof(errorsFactory));
            this.eventsService = eventsService ??
                throw new ArgumentNullException(nameof(eventsService));

        }

        /// <summary>
        /// Events Controller
        /// </summary>
        /// <param name="id"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="timetable"></param>
        /// <returns></returns>
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

            var events = eventsService.GetEvents(idValue, fromValue, toValue, timetable);

            if (events == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Student group id={id} was not found"
                );
            }

            return Content(HttpStatusCode.OK, events);
        }
    }
}
