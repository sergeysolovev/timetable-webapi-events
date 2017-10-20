using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public abstract class StudentGroupEventsViewModelBase : BreadcrumbViewModel
    {
        public int StudentGroupId { get; set; }
        public string StudentGroupDisplayName { get; set; }
        public string TimeTableDisplayName { get; set; }
        public abstract string ViewName { get; }

        public static Breadcrumb GetBreadcrumb(PublicDivision publicDivision, StudyProgram studyProgram)
        {
            return new Breadcrumb()
            {
                BreadcrumbHelper.GetBreadcrumbRootItem(false),
                BreadcrumbHelper.GetBreadcrumbPublicDivisionItem(publicDivision, false),
                BreadcrumbHelper.GetBreadcrumbCourseItem(publicDivision, studyProgram, false),
                BreadcrumbHelper.GetBreadcrumbTimeTableItem()
            };
        }

        public static string GetStudentGroupDisplayName(StudentGroup studentGroup)
        {
            if (studentGroup != null)
            {
                return String.Concat(Resources.Resources.StudentGroup, " ", studentGroup.Name);
            }
            return String.Empty;
        }
    }
}
