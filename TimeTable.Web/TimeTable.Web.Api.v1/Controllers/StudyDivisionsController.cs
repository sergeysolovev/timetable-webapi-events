using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Docs;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace SpbuEducation.TimeTable.Web.Api.v1.Controllers
{
    /// <summary>
    /// Study Divisions Controller
    /// </summary>
    [RoutePrefix(WebApi.RoutePrefix)]
    public class StudyDivisionsController : ApiController
    {
        private readonly IStudyDivisionsService divisionsService;
        private readonly IErrorsResultFactory errorsFactory;

        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="divisionsService"></param>
        /// <param name="errorsFactory"></param>
        public StudyDivisionsController(
            IStudyDivisionsService divisionsService,
            IErrorsResultFactory errorsFactory)
        {
            this.errorsFactory = errorsFactory ??
                throw new ArgumentNullException(nameof(errorsFactory));

            this.divisionsService = divisionsService ??
                throw new ArgumentNullException(nameof(divisionsService));
        }

        /// <summary>
        /// Gets study divisions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("study/divisions")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<StudyDivisionContract>),
            Description = "Study divisions were returned successfully")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult Get()
        {
            var divisions = divisionsService.Get();
            return Content(HttpStatusCode.OK, divisions);
        }

        /// <summary>
        /// Gets study programs with levels for a given study division
        /// </summary>
        /// <param name="alias">The division's short name code (alias): string</param>
        /// <returns></returns>
        [HttpGet]
        [Route("study/divisions/{alias}/programs/levels")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<StudyDivisionProgramLevelContract>),
            Description = "Study programs with levels for the division were found and returned successfully or no programs were found")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The division can not be found by a given alias")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetProgramsLevels(string alias)
        {
            var programLevels = divisionsService.GetProgramLevels(alias);

            if (programLevels == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Division '{alias}' was not found"
                );
            }

            return Content(HttpStatusCode.OK, programLevels);
        }

        #region Obsolete
        /// <summary>
        /// Gets study programs with levels for a given study division
        /// </summary>
        /// <param name="alias">The division's short name code (alias): string</param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("{alias}/studyprograms")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<StudyDivisionProgramLevelContract>),
            Description = "Study programs with levels for the division were found and returned successfully or no programs were found")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The division can not be found by a given alias")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetLevelsProgramsObsolete(string alias)
        {
            return GetProgramsLevels(alias);
        }

        /// <summary>
        /// Gets study divisions
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("divisions")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<StudyDivisionContract>),
            Description = "Study divisions were returned successfully")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetObsolete()
        {
            var divisions = divisionsService.Get();
            return Content(HttpStatusCode.OK, divisions);
        }
        #endregion
    }
}
