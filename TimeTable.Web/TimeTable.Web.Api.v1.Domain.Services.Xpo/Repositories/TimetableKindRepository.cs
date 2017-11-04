using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories
{
    internal class TimetableKindRepository : XpoRepository
    {
        public TimetableKindRepository(Session session) : base(session)
        {
        }

        public StudyEventsTimeTableKind Get(StudyEventsTimeTableKindCode timetable)
        {
            return TimeTableHelper.GetStudyEventsTimeTableKindForCode(session, timetable);
        }
    }
}
