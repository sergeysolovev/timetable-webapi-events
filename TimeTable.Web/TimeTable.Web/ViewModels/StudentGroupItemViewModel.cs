using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudentGroupItemViewModel : IViewModel
    {
        public const int MaxProfilesStringLength = 60;
        public int StudentGroupId { get; set; }
        public string StudentGroupName { get; set; }
        public string StudentGroupStudyForm { get; set; }
        public string StudentGroupProfiles { get; set; }
        public string PublicDivisionAlias { get; set; }

        public static StudentGroupItemViewModel Build(StudentGroup studentGroup, PublicDivision publicDivision)
        {
            var language = CultureHelper.CurrentLanguage;
            var studyFormName = studentGroup.StudyPlan == null ?
                String.Empty : studentGroup.StudyPlan.StudyForm.GetNameByLanguage(language);

            return new StudentGroupItemViewModel
            {
                StudentGroupId = studentGroup.Id,
                StudentGroupName = studentGroup.Name,
                StudentGroupStudyForm = studyFormName,
                StudentGroupProfiles = GetStudentGroupProfilesString(studentGroup),
                PublicDivisionAlias = publicDivision.Alias
            };
        }

        private static string GetStudentGroupProfilesString(StudentGroup studentGroup)
        {
            var language = CultureHelper.CurrentLanguage;
            var profilesString = studentGroup.GetProfilesStringByLanguage(language);
            return GetProfilesString(profilesString);
        }

        private static string GetProfilesString(string profilesString)
        {
            if (profilesString.Length < MaxProfilesStringLength)
            {
                return profilesString;
            }
            else
	        {
                return String.Format("{0}...", profilesString.Substring(0, MaxProfilesStringLength));
	        }
        }
    }
}
