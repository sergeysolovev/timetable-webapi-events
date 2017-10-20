using System;
using SpbuEducation.TimeTable.Web.API.v1.Domain.DataContracts;

namespace SpbuEducation.TimeTable.Web.API.v1.Domain.Services
{
    public interface IGroupsService
    {
        GroupEventsContract GetWeekEvents(int id, DateTime? from);
    }
}