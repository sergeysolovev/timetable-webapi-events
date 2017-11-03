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
    /// Groups Controller
    /// </summary>
    [RoutePrefix(WebApi.RoutePrefix)]
    public class GroupsController : ApiController
    {
        private readonly IGroupsService groupsService;
        private readonly IStudyDivisionsService divisionsService;
        private readonly IErrorsResultFactory errorsFactory;

        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="groupsService"></param>
        /// <param name="divisionsService"></param>
        /// <param name="errorsFactory"></param>
        public GroupsController(
            IGroupsService groupsService,
            IStudyDivisionsService divisionsService,
            IErrorsResultFactory errorsFactory)
        {
            this.groupsService = groupsService ??
                throw new ArgumentNullException(nameof(groupsService));

            this.divisionsService = divisionsService ??
                throw new ArgumentNullException(nameof(divisionsService));

            this.errorsFactory = errorsFactory ??
                throw new ArgumentNullException(nameof(errorsFactory));
        }

        /// <summary>
        /// Gets a given student group's events for the current week
        /// </summary>
        /// <param name="id">The student group's id: integer</param>
        /// <param name="timetable">Unknown = 0,Primary = 1,Attestation = 2,Final = 3</param>
        /// <returns></returns>
        [HttpGet]
        [Route("groups/{id}/events")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GroupEventsContract),
            Description = "The student group's events for the week were found and returned successfully or no events were found for the group")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "The group's id is not a valid integer")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The group can not be found by a given id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetEvents(string id, TimeTableKindСode timetable = TimeTableKindСode.Unknown)
        {
            if (!int.TryParse(id, out int idValue))
            {
                return errorsFactory.CreateBadRequest(this, id, nameof(id));
            }

            var events = groupsService.GetWeekEvents(idValue, null, timetable);

            if (events == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Student group id={id} was not found"
                );
            }

            return Content(HttpStatusCode.OK, events);
        }

        /// <summary>
        /// Gets a given student group's events for a week starting from a specified
        /// datetime until the end of the week
        /// </summary>
        /// <param name="id">The student group's id: integer</param>
        /// <param name="from">The datetime the events start from: datetime</param>
        /// <param name="timetable">Unknown = 0,Primary = 1,Attestation = 2,Final = 3</param>
        /// <returns></returns>
        [HttpGet]
        [Route("groups/{id}/events/{from}")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GroupEventsContract),
            Description = "The student group's events for the week were found and returned successfully or no events were found for the group")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "The group's id is not a valid integer or from parameter is not a valid datetime")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The group can not be found by a given id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetEvents(string id, string from, TimeTableKindСode timetable = TimeTableKindСode.Unknown)
        {
            if (!int.TryParse(id, out int idValue))
            {
                return errorsFactory.CreateBadRequest(this, id, nameof(id));
            }

            if (!DateTime.TryParse(from, out DateTime fromValue))
            {
                return errorsFactory.CreateBadRequest(this, from, nameof(from));
            }

            var events = groupsService.GetWeekEvents(idValue, fromValue, timetable);

            if (events == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Student group id={id} was not found"
                );
            }

            return Content(HttpStatusCode.OK, events);
        }

        #region Obsolete
        /// <summary>
        /// Gets a given's student group events for a week starting from a specified
        /// datetime until the end of the week
        /// </summary>
        /// <param name="divisionAlias">The study division's short name code (alias): string</param>
        /// <param name="id">The student group's id: integer</param>
        /// <param name="weekMonday">The datetime the events start from: datetime</param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("{divisionAlias}/studentgroup/{id}/events")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GroupEventsContract),
            Description = "Student group events for the week were found and returned successfully or no events were found for the group")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "The group's id is not a valid integer")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The group can not be found by a given id or the division can not be found by a given alias")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetEventsObsolete(string divisionAlias, string id, DateTime? weekMonday = null)
        {
            if (!int.TryParse(id, out int idValue))
            {
                return errorsFactory.CreateBadRequest(this, id, nameof(id));
            }

            var division = divisionsService.Get(divisionAlias);

            if (division == null)
            {
                return errorsFactory.CreateBadRequest(this,
                    $"Study division with '{divisionAlias}' was not found"
                );
            }

            var events = groupsService.GetWeekEvents(idValue, from: weekMonday);

            if (events == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Student group id={id} was not found"
                );
            }

            return Content(HttpStatusCode.OK, events);
        } 
        #endregion
    }
}
