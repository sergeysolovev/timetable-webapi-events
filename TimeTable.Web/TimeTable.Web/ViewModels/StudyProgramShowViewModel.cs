using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Data.Filtering;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Web.Helpers;
using System.Web.Routing;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudyProgramShowViewModel : BreadcrumbViewModel
    {
        public string StudyProgramDisplayText { get; set; }
        public string AdmissionYearText { get; set; }
        public string PublicDivisionAlias { get; set; }
        public IEnumerable<StudentGroupItemViewModel> StudentGroupsForCurrentStudyYear { get; set; }
        public IEnumerable<StudentGroupItemViewModel> StudentGroupsForPreviousStudyYear { get; set; }
        public string CurrentStudyYearDisplayText { get; set; }
        public string PreviousStudyYearDisplayText { get; set; }

        public static StudyProgramShowViewModel Build(IEnumerable<StudyProgram> studyPrograms, PublicDivision publicDivision)
        {
            var firstStudyProgram = studyPrograms.First();
            var session = firstStudyProgram.Session;
            
            var admissionYear = firstStudyProgram.AdmissionYear;
            var language = CultureHelper.CurrentLanguage;

            var currentStudyYear = StudyYearHelper.GetDefaultCurrentStudyYear(session);
            var previousStudyYear = StudyYearHelper.GetPreviousStudyYear(session);
            var studentGroups = GetStudentGroups(studyPrograms);
            var studentGroupsForCurrentStudyYear = studentGroups.Where(sg => sg.CurrentStudyYear == currentStudyYear);
            var studentGroupsForPreviousStudyYear = previousStudyYear.IsWebAvailable ? studentGroups.Where(sg => sg.CurrentStudyYear == previousStudyYear) : new List<StudentGroup>();

            return new StudyProgramShowViewModel
            {
                StudyProgramDisplayText = GetStudyProgramDisplayText(firstStudyProgram),
                AdmissionYearText = GetAdmissionYearText(admissionYear),
                StudentGroupsForCurrentStudyYear = studentGroupsForCurrentStudyYear.Select(sg => StudentGroupItemViewModel.Build(sg, publicDivision)),
                StudentGroupsForPreviousStudyYear = studentGroupsForPreviousStudyYear.Select(sg => StudentGroupItemViewModel.Build(sg, publicDivision)),
                CurrentStudyYearDisplayText = currentStudyYear.GetDisplayNameByLanguage(language),
                PreviousStudyYearDisplayText = previousStudyYear.GetDisplayNameByLanguage(language),
                PublicDivisionAlias = publicDivision.Alias,
                Breadcrumb = new Breadcrumb()
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(false),
                    BreadcrumbHelper.GetBreadcrumbPublicDivisionItem(publicDivision, false),
                    BreadcrumbHelper.GetBreadcrumbCourseItem(publicDivision, firstStudyProgram, true),
                }
            };
        }

        public static IEnumerable<StudentGroup> GetStudentGroups(IEnumerable<StudyProgram> studyPrograms)
        {
            return studyPrograms.SelectMany(sp => sp.ContingentUnits)
                    .Where(cu => cu is StudentGroup && !cu.IsArchived)
                    .Cast<StudentGroup>()
                    .OrderBy(sg => sg.StudyPlan == null ? String.Empty : sg.StudyPlan.StudyForm.Name)
                    .ThenBy(sg => sg.Name);
        }

        private static string GetAdmissionYearText(StudyYear admissionYear)
        {
            if (admissionYear != null)
            {
                return String.Concat(admissionYear.Name, " ", Resources.Resources.yearOfAdmission);
            }
            return String.Empty;
        }

        private static string GetStudyProgramDisplayText(StudyProgram studyProgram)
        {
            var language = CultureHelper.CurrentLanguage;
            return String.Concat(Resources.Resources.StudyProgram, " ", studyProgram.GetNameByLanguage(language));
        }
    }
}

