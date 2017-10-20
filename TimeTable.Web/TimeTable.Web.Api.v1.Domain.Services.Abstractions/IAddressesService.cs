using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions
{
    /// <summary>
    /// Addresses Service
    /// </summary>
    public interface IAddressesService
    {
        /// <summary>
        /// Gets addresses by criteria
        /// </summary>
        /// <param name="equipmentElements"></param>
        /// <param name="seating"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        IEnumerable<AddressContract> Get(
            IEnumerable<string> equipmentElements,
            Seating? seating = default(Seating?),
            int? capacity = default(int?)
        );

        /// <summary>
        /// Gets classrooms for address by criteria
        /// </summary>
        /// <param name="oid">address oid</param>
        /// <param name="equipmentElements"></param>
        /// <param name="seating"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        IEnumerable<ClassroomContract> GetClassrooms(
            Guid oid,
            IEnumerable<string> equipmentElements,
            Seating? seating = default(Seating?),
            int? capacity = default(int?)
        );
    }
}