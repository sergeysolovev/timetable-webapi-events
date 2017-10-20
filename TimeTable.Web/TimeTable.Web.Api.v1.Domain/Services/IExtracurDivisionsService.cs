using System.Collections.Generic;
using SpbuEducation.TimeTable.Web.API.v1.Domain.DataContracts;
using System;

namespace SpbuEducation.TimeTable.Web.API.v1.Domain.Services
{
    public interface IExtracurDivisionsService
    {
        IEnumerable<ExtracurDivisionContract> Get();
        ExtracurEventsContract GetEvents(string alias, DateTime? fromDate = null);
    }
}