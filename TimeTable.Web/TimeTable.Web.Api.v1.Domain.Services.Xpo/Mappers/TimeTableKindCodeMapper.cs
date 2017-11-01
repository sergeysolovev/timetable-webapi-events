using System;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers
{
    internal class TimeTableKindCodeMapper
    {
        public TimeTableKindCodeMapper()
                    { }
        
                public StudyEventsTimeTableKindCode? Map(TimeTableKindСode? timetable)
            {
                switch (timetable)
               {
                    case null:
                        return null;
                    case TimeTableKindСode.Unknown:
                        return StudyEventsTimeTableKindCode.Unknown;
                    case TimeTableKindСode.Primary:
                        return StudyEventsTimeTableKindCode.Primary;
                    case TimeTableKindСode.Attestation:
                        return StudyEventsTimeTableKindCode.Attestation;
                    case TimeTableKindСode.Final:
                        return StudyEventsTimeTableKindCode.Final;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(timetable));
                }
        } 
    }
}
