using System;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions
{
    /// <summary>
    /// Classrooms Service
    /// </summary>
    public interface IClassroomsService
    {
        /// <summary>
        /// Determines whether a classroom is busy in a given interval
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        ClassroomBusynessContract IsBusy(Guid oid, DateTime from, DateTime to);
    }
}