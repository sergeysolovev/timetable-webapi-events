using System;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions
{
    /// <summary>
    /// Student Groups Service
    /// </summary>
    public interface IGroupsService
    {
        /// <summary>
        /// Gets events for the current week or a week, determined by a date
        /// </summary>
        /// <param name="id"></param>
        /// <param name="from"></param>
        /// <param name="timeTableKind"></param>
        /// <returns></returns>
        GroupEventsContract GetWeekEvents(int id, DateTime? from = null, TimeTableKindСode timeTableKind = TimeTableKindСode.Unknown);
    }
}