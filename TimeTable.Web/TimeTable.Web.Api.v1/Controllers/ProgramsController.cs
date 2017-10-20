using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Docs;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SpbuEducation.TimeTable.Web.Api.v1.Controllers
{
    /// <summary>
    /// Programs Controller
    /// </summary>
    [RoutePrefix(WebApi.RoutePrefix)]
    public class ProgramsController : ApiController
    {
        private readonly IProgramsService programsService;
        private readonly IStudyDivisionsService divisionsService;
        private readonly IErrorsResultFactory errorsFactory;

        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="programsService"></param>
        /// <param name="divisionsService"></param>
        /// <param name="errorsFactory"></param>
        public ProgramsController(
            IProgramsService programsService,
            IStudyDivisionsService divisionsService,
            IErrorsResultFactory errorsFactory)
        {
            this.programsService = programsService ??
                throw new ArgumentNullException(nameof(programsService));

            this.divisionsService = divisionsService ??
                throw new ArgumentNullException(nameof(divisionsService));

            this.errorsFactory = errorsFactory ??
                throw new ArgumentNullException(nameof(errorsFactory));
        }

        /// <summary>
        /// Gets a given study program's student groups for the current study year
        /// </summary>
        /// <param name="id">The study program's id: integer</param>
        /// <returns></returns>
        [HttpGet]
        [Route("progams/{id}/groups")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProgramGroupsContract),
            Description = "Student groups for the division and the current study year were found and returned successfully or no groups were found for the program")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "The programs's id is not a valid integer")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The program can not be found by a given id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetGroups(string id)
        {
            if (!int.TryParse(id, out int idValue))
            {
                return errorsFactory.CreateBadRequest(this, id, nameof(id));
            }

            var groups = programsService.GetGroups(idValue);

            if (groups == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Study Program id={id} was not found"
                );
            }

            return Content(HttpStatusCode.OK, groups);
        }

        #region Obsolete
        /// <summary>
        /// Gets a given study program's student groups for the current study year
        /// </summary>
        /// <param name="id">The study program's id: integer</param>
        /// <param name="alias">The study division's short name code (alias): string</param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("{alias}/studyprogram/{id}/studentgroups")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProgramGroupsContract),
            Description = "Student groups for the division and the current study year were found and returned successfully or no groups were found for the program")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "The programs's id is not a valid integer")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The program can not be found by a given id or the division can not be found by a given alias")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetGroupsObsolete(string alias, string id)
        {
            if (!int.TryParse(id, out int idValue))
            {
                return errorsFactory.CreateBadRequest(this, id, nameof(id));
            }

            var division = divisionsService.Get(alias);

            if (division == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"The study division '{alias}' was not found"
                );
            }

            var groups = programsService.GetGroups(idValue);

            if (groups == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Study Program id={id} was not found"
                );
            }

            // for backwards compatibility
            // set division alias (outside of programs service)
            var groupsWithAliases = groups.Groups.Select(g => new ProgramGroupsContract.Group
            {
                Id = g.Id,
                Name = g.Name,
                Form = g.Form,
                Profiles = g.Profiles,
                DivisionAlias = division.Alias
            });

            return Content(HttpStatusCode.OK, groupsWithAliases);
        }
        #endregion
    }
}
