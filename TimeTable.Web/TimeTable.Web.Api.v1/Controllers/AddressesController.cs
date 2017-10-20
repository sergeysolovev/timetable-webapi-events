using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Docs;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using SpbuEducation.TimeTable.Web.Api.v1.Http.ModelBinding;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SpbuEducation.TimeTable.Web.Api.v1.Controllers
{
    /// <summary>
    /// Addresses controller
    /// </summary>
    [RoutePrefix(WebApi.RoutePrefix)]
    public class AddressesController : ApiController
    {
        private readonly IAddressesService addressesService;
        private readonly IErrorsResultFactory errorsFactory;
        private readonly ParameterParser paramParser;

        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="addressesService"></param>
        /// <param name="errorsFactory"></param>
        /// <param name="paramParser"></param>
        public AddressesController(
            IAddressesService addressesService,
            IErrorsResultFactory errorsFactory,
            ParameterParser paramParser)
        {
            this.addressesService = addressesService ??
                throw new ArgumentNullException(nameof(addressesService));

            this.errorsFactory = errorsFactory ??
                throw new ArgumentNullException(nameof(errorsFactory));

            this.paramParser = paramParser ??
                throw new ArgumentNullException(nameof(paramParser));
        }

        /// <summary>
        /// Gets addresses filtered by a given optional criteria
        /// </summary>
        /// <param name="seating">
        /// Seating type: theater = 0, amphitheater = 1, roundtable = 2.
        /// Uses any seating type if not specified
        /// </param>
        /// <param name="capacity">Minimal capacity: integer</param>
        /// <param name="equipment">Equipment: comma-separated values list</param>
        /// <returns></returns>
        [HttpGet]
        [Route("addresses")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<AddressContract>),
            Description = "Addresses were found and returned successfully or no addresses were found by the given criteria")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "Parameter seating or capacity has invalid value")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult Get(
            string seating = null,
            string capacity = null,
            string equipment = null)
        {
            if (!paramParser.TryParseNullableSeating(seating, out Seating? seatingValue))
            {
                return errorsFactory.CreateBadRequest(this, seating, nameof(seating));
            }

            if (!paramParser.TryParseNullableInt32(capacity, out int? minCapacityValue))
            {
                return errorsFactory.CreateBadRequest(this, capacity, nameof(capacity));
            }

            var equipmentElements = GetEquipmentElements(equipment);
            var addresses = addressesService.Get(equipmentElements, seatingValue, minCapacityValue);

            return Content(HttpStatusCode.OK, addresses);
        }

        /// <summary>
        /// Gets classrooms by a given optional criteria
        /// </summary>
        /// <param name="oid">The address id: GUID</param>
        /// <param name="seating">
        /// Seating type: theater = 0, amphitheater = 1, roundtable = 2.
        /// Uses any seating type if not specified
        /// </param>
        /// <param name="minCapacity">Minimal capacity: integer</param>
        /// <param name="equipment">Equipment: comma-separated values list</param>
        /// <returns></returns>
        [HttpGet]
        [Route("addresses/{oid}/classrooms")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<ClassroomContract>),
            Description = "Classrooms were found and returned successfully or no classrooms were found by the given criteria")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "Parameter oid, seating or capacity has invalid value")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The address can not be found by a given oid")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetClassrooms(
            string oid,
            string seating = null,
            string minCapacity = null,
            string equipment = null)
        {
            if (!Guid.TryParse(oid, out Guid oidValue))
            {
                return errorsFactory.CreateBadRequest(this, oid, nameof(oid));
            }

            if (!paramParser.TryParseNullableSeating(seating, out Seating? seatingValue))
            {
                return errorsFactory.CreateBadRequest(this, seating, nameof(seating));
            }

            if (!paramParser.TryParseNullableInt32(minCapacity, out int? minCapacityValue))
            {
                return errorsFactory.CreateBadRequest(this, minCapacity, nameof(minCapacity));
            }

            var equipmentElements = GetEquipmentElements(equipment);
            try
            {
                var classrooms = addressesService.GetClassrooms(
                    oidValue,
                    equipmentElements,
                    seatingValue,
                    minCapacityValue
                );

                if (classrooms == null)
                {
                    return errorsFactory.CreateNotFound(this,
                        $"Address oid='{oid}' was not found"
                    );
                }

                return Content(HttpStatusCode.OK, classrooms);
            }
            catch (ArgumentOutOfRangeException ex) when (ex.ParamName == nameof(seating))
            {
                return errorsFactory.CreateBadRequest(this,
                    $"Parameter seating: value '{seating}' is not supported by this API"
                );
            }
        }

        #region Obsolete
        /// <summary>
        /// Gets classrooms by a given optional criteria
        /// </summary>
        /// <param name="oid">The address id: GUID</param>
        /// <param name="seating">
        /// Seating type: theater = 0, amphitheater = 1, roundtable = 2.
        /// Uses any seating type if not specified
        /// </param>
        /// <param name="capacity">Minimal capacity</param>
        /// <param name="equipment">Equipment: comma-separated values list</param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("address/{oid}/locations")]
        [SwaggerProduces("application/json", "application/xml")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<ClassroomContract>),
            Description = "Classrooms were found and returned successfully")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ErrorsContract),
            Description = "Parameter oid has invalid value")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(ErrorsContract),
            Description = "The address can not be found by a given oid or no classrooms were found by the given criteria")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(ErrorsContract),
            Description = "Something went wrong")]
        public IHttpActionResult GetClassroomsObsolete(
            string oid,
            Seating? seating = null,
            int? capacity = null,
            string equipment = null)
        {
            if (!Guid.TryParse(oid, out Guid oidValue))
            {
                return errorsFactory.CreateBadRequest(this, oid, nameof(oid));
            }

            var equipmentElements = GetEquipmentElements(equipment);
            try
            {
                var classrooms = addressesService.GetClassrooms(
                    oidValue,
                    equipmentElements,
                    seating,
                    capacity
                );

                if (classrooms == null)
                {
                    return errorsFactory.CreateNotFound(this,
                        $"Address oid='{oid}' was not found"
                    );
                }

                if (!classrooms.Any())
                {
                    return errorsFactory.CreateNotFound(this,
                        $"No classrooms where found by the given criteria"
                    );
                }

                return Content(HttpStatusCode.OK, classrooms);
            }
            catch (ArgumentOutOfRangeException ex) when (ex.ParamName == nameof(seating))
            {
                return errorsFactory.CreateBadRequest(this,
                    $"Parameter seating: value '{seating}' is not supported by this API"
                );
            }
        }
        #endregion

        #region Helpers
        private IEnumerable<string> GetEquipmentElements(string equipment)
        {
            return (equipment ?? string.Empty)
                .Split(new[] { ", ", "," }, StringSplitOptions.None)
                .Where(e => !string.IsNullOrEmpty(e));
        }
        #endregion
    }
}
