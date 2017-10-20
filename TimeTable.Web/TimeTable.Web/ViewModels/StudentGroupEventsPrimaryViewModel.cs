using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudentGroupEventsPrimaryViewModel : StudentGroupEventsViewModelBase
    {
        public string PreviousWeekMonday { get; set; }
        public string NextWeekMonday { get; set; }
        public bool IsPreviousWeekReferenceAvailable { get { return !String.IsNullOrEmpty(PreviousWeekMonday); } }
        public bool IsNextWeekReferenceAvailable { get { return !String.IsNullOrEmpty(NextWeekMonday); } }
        public bool IsCurrentWeekReferenceAvailable { get; set; }
        public string WeekDisplayText { get; set; }
        public IEnumerable<StudyEventDayItemViewModel> Days { get; set; }

        public override string ViewName
        {
            get { return "Primary"; }
        }

        public string WeekMonday { get; set; }

        public static StudentGroupEventsPrimaryViewModel Build(StudentGroup studentGroup, PublicDivision publicDivision, DateTime? weekMonday)
        {
            var defaultWeekStart = DateTimeHelper.GetWeekStart(DateTime.Today);
            var weekStart = weekMonday ?? defaultWeekStart;
            var weekEnd = weekStart.AddDays(7);
            var language = CultureHelper.CurrentLanguage;

            return new StudentGroupEventsPrimaryViewModel
            {
                StudentGroupId = studentGroup.Id,
                StudentGroupDisplayName = GetStudentGroupDisplayName(studentGroup),
                TimeTableDisplayName = TimeTableHelper.GetStudentGroupTimeTableDisplayNameForCodeByLanguage(StudyEventsTimeTableKindCode.Primary, language),
                WeekDisplayText = DateTimeHelper.GetWeekDisplayText(language, weekStart, weekEnd),
                PreviousWeekMonday = DateTimeHelper.GetDateStringForWeb(weekStart.AddDays(-7)),
                WeekMonday = DateTimeHelper.GetDateStringForWeb(weekStart),
                NextWeekMonday = DateTimeHelper.GetDateStringForWeb(weekEnd),
                IsCurrentWeekReferenceAvailable = (defaultWeekStart != weekStart),
                Days = StudyEventsViewModelHelper.GetStudyEventsDaysViewModelsForDateRange(studentGroup, null, weekStart, weekEnd),
                Breadcrumb = GetBreadcrumb(publicDivision, studentGroup.StudyProgram)
            };
        }
    }
}
