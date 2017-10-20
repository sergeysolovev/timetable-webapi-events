using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Docs;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Globalization;
using System.Net;
using System.Web.Http;

namespace SpbuEducation.TimeTable.Web.Api.v1.Controllers
{
    /// <summary>
    /// Classrooms Controller
    /// </summary>
    [RoutePrefix(WebApi.RoutePrefix)]
    public class ClassroomsController : ApiController
    {
        private readonly IClassroomsService classroomsService;
        private readonly IErrorsResultFactory errorsFactory;

        /// <summary>
        /// Contructor injection
        /// </summary>
        /// <param name="classroomsService"></param>
        /// <param name="errorsFactory"></param>
        public ClassroomsController(
            IClassroomsService classroomsService,
            IErrorsResultFactory errorsFactory)
        {
            this.classroomsService = classroomsService ??
                throw new ArgumentNullException(nameof(classroomsService));

            this.errorsFactory = errorsFactory ??
                throw new ArgumentNullException(nameof(errorsFactory));
        }

        /// <summary>
        /// Checks whether a given classroom is busy in a specified interval or it's part
        /// </summary>
        /// <param name="oid">The classroom's id: integer</param>
        /// <param name="start">The interval's start, format: 'yyyyMMddHHmm'</param>
        /// <param name="end">The interval's end, format: 'yyyyMMddHHmm'</param>
        [HttpGet]
        [Route("classrooms/{oid}/isbusy/{start}/{end}")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ClassroomBusynessContract),
            Description = "The classroom and it's busyness info were found and returned successfully")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "Parameter oid, start or end has invalid value")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The classroom can not be found by a given oid")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult IsBusy(string oid, string start, string end)
        {
            if (!Guid.TryParse(oid, out Guid oidValue))
            {
                return errorsFactory.CreateBadRequest(this, oid, nameof(oid));
            }

            if (!parseDateTime(start, out DateTime startValue))
            {
                return errorsFactory.CreateBadRequest(this, start, nameof(start));
            }

            if (!parseDateTime(end, out DateTime endValue))
            {
                return errorsFactory.CreateBadRequest(this, end, nameof(end));
            }

            var contract = classroomsService.IsBusy(oidValue, startValue, endValue);

            if (contract == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Classroom oid='{oid}' was not found"
                );
            }

            return Content(HttpStatusCode.OK, contract);

            bool parseDateTime(string value, out DateTime result)
                => DateTime.TryParseExact(value, "yyyyMMddHHmm", null, DateTimeStyles.None, out result);
        }

        #region Obsolete
        /// <summary>
        /// Checks whether a given classroom is busy in a specified interval or it's part
        /// </summary>
        /// <param name="oid">The classroom's id: integer</param>
        /// <param name="start">The interval's start in a given format, default 'yyyyMMddHHmm'</param>
        /// <param name="end">The interval's end in a given format, default 'yyyyMMddHHmm'</param>
        /// <param name="format">datetime format, default 'yyyyMMddHHmm'</param>
        [Obsolete]
        [HttpGet]
        [Route("location/{oid}/isreserved/{start}/{end}")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(bool),
            Description = "The classroom and it's busyness info were found and returned successfully")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "Parameter oid, start or end has invalid value")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The classroom can not be found by a given oid")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult IsReserved(string oid, string start, string end, string format = "yyyyMMddHHmm")
        {
            if (!Guid.TryParse(oid, out Guid oidValue))
            {
                return errorsFactory.CreateBadRequest(this, oid, nameof(oid));
            }

            if (!parseDateTime(start, out DateTime startValue))
            {
                return errorsFactory.CreateBadRequest(this, start, nameof(start));
            }

            if (!parseDateTime(end, out DateTime endValue))
            {
                return errorsFactory.CreateBadRequest(this, end, nameof(end));
            }

            var contract = classroomsService.IsBusy(oidValue, startValue, endValue);

            if (contract == null)
            {
                return errorsFactory.CreateNotFound(this,
                    $"Classroom oid='{oid}' was not found"
                );
            }

            return Content(HttpStatusCode.OK, contract.IsBusy);

            bool parseDateTime(string value, out DateTime result)
                => DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out result);
        }
        #endregion
    }
}
