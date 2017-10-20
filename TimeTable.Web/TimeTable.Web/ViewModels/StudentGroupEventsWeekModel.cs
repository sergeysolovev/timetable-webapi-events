using System;
using System.Collections.Generic;
using System.Linq;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudentGroupEventsWeekModel
    {
        public IEnumerable<StudyEventDayItemViewModel> Days { get; set; }
        public bool HasEvents { get; private set; }

        internal static StudentGroupEventsWeekModel Build(StudentGroup studentGroup, DateTime from, DateTime to)
        {
            var days = StudyEventsViewModelHelper.GetStudyEventsDaysViewModelsForDateRange(studentGroup, null, from, to).ToList();

            return new StudentGroupEventsWeekModel
            {
                StudentGroupId = studentGroup.Id,
                StudentGroupDisplayName = studentGroup.Name,
                TimeTableDisplayName = TimeTableHelper.GetStudentGroupTimeTableDisplayNameForCodeByLanguage(StudyEventsTimeTableKindCode.Primary, CultureHelper.CurrentLanguage),
                Days = days,
                DurationDisplayText = $"{from:d MMMM yyyy} - {to:d MMMM yyyy}",
                HasEvents = days.Any(seadivm => seadivm.DayStudyEvents.Any()),
            };
        }

        public string TimeTableDisplayName { get; set; }

        public int StudentGroupId { get; set; }

        public string StudentGroupDisplayName { get; set; }

        public string DurationDisplayText { get; set; }
    }
}
