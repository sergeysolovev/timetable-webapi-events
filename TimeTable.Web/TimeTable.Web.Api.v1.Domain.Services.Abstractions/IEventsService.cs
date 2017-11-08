using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions
{
    public interface IEventsService
    {
        GroupEventsContract GetEvents(int id, DateTime from, DateTime to,
            TimeTableKindСode localTimeTableKindCode = TimeTableKindСode.Unknown);
    }
}
