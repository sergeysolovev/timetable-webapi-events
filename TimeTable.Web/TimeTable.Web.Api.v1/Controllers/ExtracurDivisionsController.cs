using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Docs;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using SpbuEducation.TimeTable.Web.Api.v1.Http.ModelBinding;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace SpbuEducation.TimeTable.Web.Api.v1.Controllers
{
    /// <summary>
    /// Extracurricular Divisions Controller
    /// </summary>
    [RoutePrefix(WebApi.RoutePrefix)]
    public class ExtracurDivisionsController : ApiController
    {
        private readonly IExtracurDivisionsService divisionsService;
        private readonly IErrorsResultFactory errorsFactory;
        private readonly ParameterParser paramParser;

        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="divisionsService"></param>
        /// <param name="errorsFactory"></param>
        /// <param name="paramParser"></param>
        public ExtracurDivisionsController(
            IExtracurDivisionsService divisionsService,
            IErrorsResultFactory errorsFactory,
            ParameterParser paramParser)
        {
            this.divisionsService = divisionsService ??
                throw new ArgumentNullException(nameof(divisionsService));

            this.errorsFactory = errorsFactory ??
                throw new ArgumentNullException(nameof(errorsFactory));

            this.paramParser = paramParser ??
                throw new ArgumentNullException(nameof(paramParser));
        }

        /// <summary>
        /// Gets extracurricular divisions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("extracur/divisions")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<ExtracurDivisionContract>),
            Description = "Extracurricular divisions were returned successfully")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult Get()
        {
            var divisions = divisionsService.Get();
            return Content(HttpStatusCode.OK, divisions);
        }

        /// <summary>
        /// Get extracurricular events for a given division
        /// </summary>
        /// <param name="alias">The division's short name code (alias): string</param>
        /// <param name="fromDate">The date the events start from: datetime</param>
        /// <returns></returns>
        [HttpGet]
        [Route("extracur/divisions/{alias}/events")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ExtracurEventsContract),
            Description = "Events for the division were found and returned successfully or no events were found for the division")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "The division with a given alias is not supported by this API or fromDate parameter was not in a correct format")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The division can not be found by a given alias")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetEvents(string alias, string fromDate = null)
        {
            if (!paramParser.TryParseNullableDateTime(fromDate, out DateTime? fromDateValue))
            {
                return errorsFactory.CreateBadRequest(this, fromDate, nameof(fromDate));
            }

            try
            {
                var events = divisionsService.GetEvents(alias, fromDateValue);

                if (events == null)
                {
                    return errorsFactory.CreateNotFound(this,
                        $"Extracur division '{alias}' was not found"
                    );
                }

                return Content(HttpStatusCode.OK, events);
            }
            catch (NotSupportedException ex)
            {
                return errorsFactory.CreateBadRequest(this, ex.Message);
            }
        }

        #region Obsolete
        /// <summary>
        /// Gets extracurricular divisions
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("xtracurdivisions")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<ExtracurDivisionContract>),
            Description = "Extracurricular divisions were returned successfully")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetObsolete()
        {
            return Get();
        }

        /// <summary>
        /// Get extracurricular events for a given division
        /// </summary>
        /// <param name="alias">The division's short name code (alias): string</param>
        /// <param name="fromDate">The date the events start from: datetime</param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("{alias}/events")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ExtracurEventsContract),
            Description = "Events for the division were found and returned successfully or no events were found for the division")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "The division with a given alias is not supported by this API")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The division can not be found by a given alias")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetEventsObsolete(string alias, DateTime? fromDate = null)
        {
            try
            {
                var events = divisionsService.GetEvents(alias, fromDate);

                if (events == null)
                {
                    return errorsFactory.CreateNotFound(this,
                        $"Extracur division '{alias}' was not found"
                    );
                }

                return Content(HttpStatusCode.OK, events);
            }
            catch (NotSupportedException ex)
            {
                return errorsFactory.CreateBadRequest(this, ex.Message);
            }
        }
        #endregion
    }
}
