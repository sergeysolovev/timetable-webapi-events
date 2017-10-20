using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Docs;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using SpbuEducation.TimeTable.Web.Api.v1.Http.ModelBinding;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SpbuEducation.TimeTable.Web.Api.v1.Controllers
{
    /// <summary>
    /// Educators Controller
    /// </summary>
    [RoutePrefix(WebApi.RoutePrefix)]
    public class EducatorsController : ApiController
    {
        private readonly IEducatorsService educatorsService;
        private readonly IErrorsResultFactory errorsFactory;
        private readonly ParameterParser paramParser;

        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="educatorsService"></param>
        /// <param name="errorsFactory"></param>
        /// <param name="paramParser"></param>
        public EducatorsController(
            IEducatorsService educatorsService,
            IErrorsResultFactory errorsFactory,
            ParameterParser paramParser)
        {
            this.educatorsService = educatorsService ??
                throw new ArgumentNullException(nameof(educatorsService));

            this.errorsFactory = errorsFactory ??
                throw new ArgumentNullException(nameof(errorsFactory));

            this.paramParser = paramParser ??
                throw new ArgumentNullException(nameof(paramParser));
        }

        /// <summary>
        /// Gets an educator's events for the current study term
        /// </summary>
        /// <param name="id">The educator's id: integer</param>
        /// <param name="showNextTerm">Whether to show the events for the next term: integer</param>
        [HttpGet]
        [Route("educators/{id}/events")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EducatorEventsContract),
            Description = "The educator's events were found and returned successfully or no events were found for the educator")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "Parameter id or showNextTerm has invalid value")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The educator can not be found by a given id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetEvents(string id, string showNextTerm = null)
        {
            if (!int.TryParse(id, out int idValue))
            {
                return errorsFactory.CreateBadRequest(this, id, nameof(id));
            }

            if (!paramParser.TryParseNullableInt32(showNextTerm, out int? showNextTermValue))
            {
                return errorsFactory.CreateBadRequest(this, showNextTerm, nameof(showNextTerm));
            }

            var events = educatorsService.GetEvents(idValue, showNextTermValue);

            if (events == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Educator id={id} was not found"
                );
            }

            return Content(HttpStatusCode.OK, events);
        }

        /// <summary>
        /// Gets educators by searching their's last name or a part of last name
        /// </summary>
        /// <param name="query">The last name search query: non-empty string</param>
        /// <returns></returns>
        [HttpGet]
        [Route("educators/search/{query?}")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EducatorEventsContract),
            Description = "The educators were found and returned successfully or no educators were found by the given criteria")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "Parameter query has invalid value")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult SearchByLastName(string query = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                return errorsFactory.CreateBadRequest(this, query, nameof(query));
            }

            var educators = educatorsService.SearchByLastName(query);
            return Content(HttpStatusCode.OK, educators);
        }

        #region Obsolete
        /// <summary>
        /// Gets an educator's events for the current study term
        /// </summary>
        /// <param name="id">The educator's id: integer</param>
        /// <param name="next">Whether to show the events for the next term: integer</param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("educator/{id}/events")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EducatorEventsContract),
            Description = "The educator's events were found and returned successfully or no events were found for the educator")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "Parameter id has invalid value")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The educator can not be found by a given id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetEventsObsolete(string id, int? next = null)
        {
            if (!int.TryParse(id, out int idValue))
            {
                return errorsFactory.CreateBadRequest(this, id, nameof(id));
            }

            var events = educatorsService.GetEvents(idValue, next);

            if (events == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Educator id={id} was not found"
                );
            }

            return Content(HttpStatusCode.OK, events);
        }

        /// <summary>
        /// Gets educators by searching theirs last name or a part of last name
        /// </summary>
        /// <param name="q">The last name search query: string</param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("educators")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EducatorEventsContract),
            Description = "The educators were found and returned successfully or no educators were found by the given criteria")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult SearchByLastNameObsolete(string q)
        {
            var educators = educatorsService.SearchByLastName(q);

            // for backwards compatibility
            if (!educators.Educators.Any())
            {
                educators.Educators = null;
            }

            return Content(HttpStatusCode.OK, educators);
        }
        #endregion
    }
}
