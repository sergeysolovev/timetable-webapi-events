using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions
{
    /// <summary>
    /// Extracurricular Divisions Service
    /// </summary>
    public interface IExtracurDivisionsService
    {
        /// <summary>
        /// Gets extracurricular divisions
        /// </summary>
        /// <returns></returns>
        IEnumerable<ExtracurDivisionContract> Get();

        /// <summary>
        /// Gets events for extracurricular division
        /// started from a given date
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        ExtracurEventsContract GetEvents(string alias, DateTime? fromDate = null);
    }
}