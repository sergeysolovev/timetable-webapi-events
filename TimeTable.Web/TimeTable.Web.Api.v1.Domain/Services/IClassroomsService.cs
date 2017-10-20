using System;
using System.Collections.Generic;
using SpbuEducation.TimeTable.Web.API.v1.Domain.DataContracts;

namespace SpbuEducation.TimeTable.Web.API.v1.Domain.Services
{
    public interface IClassroomsService
    {
        IEnumerable<AddressContract> Get(
            IEnumerable<string> equipmentElements,
            int? seating = default(int?),
            int? capacity = default(int?)
        );

        IEnumerable<ClassroomContract> GetClassrooms(
            Guid oid,
            IEnumerable<string> equipmentElements,
            int? seating = default(int?),
            int? capacity = default(int?)
        );

        ClassroomBusyContract IsBusy(Guid oid, DateTime from, DateTime to);
    }
}