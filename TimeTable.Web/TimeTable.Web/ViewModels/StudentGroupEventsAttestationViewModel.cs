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
using SpbuEducation.TimeTable.Common.Web.ViewModels;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudentGroupEventsAttestationViewModel : StudentGroupEventsViewModelBase
    {
        public IEnumerable<StudyEventMonthViewModel> StudyEventsMonths { get; set; }

        public override string ViewName
        {
            get { return "Attestation"; }
        }
        public StudyEventsTimeTableKindCode StudyEventsTimeTableKindCode { get; set; }

        public static StudentGroupEventsAttestationViewModel Build(StudentGroup studentGroup, PublicDivision publicDivision, StudyEventsTimeTableKindCode studyEventsTimeTableKindCode)
        {
            var language = CultureHelper.CurrentLanguage;
            var studyEventsTimeTableKind = TimeTableHelper.GetStudyEventsTimeTableKindForCode(
                studentGroup.Session,
                studyEventsTimeTableKindCode);
            return new StudentGroupEventsAttestationViewModel
            {
                StudentGroupId = studentGroup.Id,
                StudentGroupDisplayName = GetStudentGroupDisplayName(studentGroup),
                TimeTableDisplayName = TimeTableHelper.GetStudentGroupTimeTableDisplayNameForCodeByLanguage(studyEventsTimeTableKindCode, language),
                StudyEventsMonths = StudyEventsViewModelHelper.GetStudyEventsMonthsViewModelsForTerm(studentGroup, studyEventsTimeTableKind, studyEventsTimeTableKindCode),
                Breadcrumb = GetBreadcrumb(publicDivision, studentGroup.StudyProgram),
                StudyEventsTimeTableKindCode = studyEventsTimeTableKindCode
            };
        }

    }
}
