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
    public class StudentGroupEventsSemesterViewModel : StudentGroupEventsViewModelBase
    {
        public IEnumerable<StudyEventAggregatedDayItemViewModel> Days { get; set; }
        public DateTime SemesterStartDate { get; private set; }
        public DateTime SemesterEndDate { get; set; }
        public string SemesterDurationDisplayText { get; set; }
        public bool IsSpringSemester { get; set; }
        public bool HasEvents { get; private set; }

        public override string ViewName
        {
            get { return "Semester"; }
        }

        public string PublicDivisionAlias { get; private set; }
        public int? Autumn { get; private set; }

        public static StudentGroupEventsSemesterViewModel Build(StudentGroup studentGroup, PublicDivision publicDivision, int? autumn)
        {
            var autumnSemesterStartDate = new DateTime(studentGroup.CurrentStudyYear.Number, 8, 1);

            var semesterStartDate = (autumn == 1 || autumn == null && NowIsAutumnSemester())
                ? autumnSemesterStartDate
                : autumnSemesterStartDate.AddMonths(6);

            var semesterEndDate = semesterStartDate.AddMonths(6);

            var days = StudyEventsViewModelHelper.GetStudentGroupAggregatedEventsDays(studentGroup, null, semesterStartDate, semesterEndDate).ToList();

            return new StudentGroupEventsSemesterViewModel
            {
                Autumn = autumn,
                PublicDivisionAlias = publicDivision.Alias,
                StudentGroupId = studentGroup.Id,
                StudentGroupDisplayName = GetStudentGroupDisplayName(studentGroup),
                TimeTableDisplayName = TimeTableHelper.GetStudentGroupTimeTableDisplayNameForCodeByLanguage(StudyEventsTimeTableKindCode.Primary, CultureHelper.CurrentLanguage),
                Days = days,
                Breadcrumb = GetBreadcrumb(publicDivision, studentGroup.StudyProgram),
                SemesterStartDate = semesterStartDate,
                SemesterEndDate = semesterEndDate,
                SemesterDurationDisplayText = $"{semesterStartDate:d MMMM yyyy} - {semesterEndDate:d MMMM yyyy}",
                IsSpringSemester = semesterStartDate.Month == 2,
                HasEvents = days.Any(seadivm => seadivm.DayStudyEvents.Any()),
            };
        }

        private static bool NowIsAutumnSemester()
        {
            return DateTime.Now.Month > 8 || DateTime.Now.Month < 2;
        }
    }
}
