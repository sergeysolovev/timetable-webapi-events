using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace SpbuEducation.TimeTable.Web.Helpers
{
    public class BreadcrumbHelper
    {
        public static BreadcrumbItem GetBreadcrumbRootItem(bool isActive)
        {
            return new BreadcrumbItem
            {
                IsActive = isActive,
                DisplayText = Resources.Resources.BreadcrumbRoot,
                ControllerName = "Home",
                ActionName = "Index"
            };
        }

        public static BreadcrumbItem GetBreadcrumbEducatorsItem(bool isActive)
        {
            return new BreadcrumbItem
            {
                IsActive = isActive,
                DisplayText = Resources.Resources.Educators,
                ControllerName = "EducatorEvents",
                ActionName = "Index"
            };
        }

        public static BreadcrumbItem GetBreadcrumbXtracurEventsItem(bool isActive)
        {
            return new BreadcrumbItem
            {
                IsActive = isActive,
                DisplayText = Resources.Resources.Extracurricular,
                ControllerName = "Home",
                ActionName = "Index"
            };
        }

        public static BreadcrumbItem GetBreadcrumbXtracurEventsItem(XtracurDivision xtracurDivision, bool isActive)
        {
            LanguageCode language = CultureHelper.CurrentLanguage;
            return new BreadcrumbItem
            {
                IsActive = isActive,
                DisplayText = xtracurDivision.GetNameByLanguage(language),
                ControllerName = "XtracurEvents",
                ActionName = "Index",
                RouteValues = new RouteValueDictionary(new Dictionary<string, object> {
                    { "alias", xtracurDivision.Alias }
                })
            };
        }

        public static BreadcrumbItem GetBreadcrumbXtracurSearchItem(bool isActive)
        {
            return new BreadcrumbItem
            {
                IsActive = isActive,
                DisplayText = Resources.Resources.Search,
                ControllerName = "XtracurEvents",
                ActionName = "Search"
            };
        }

        public static BreadcrumbItem GetBreadcrumbPublicDivisionItem(PublicDivision publicDivision, bool isActive)
        {
            var language = CultureHelper.CurrentLanguage;
            return new BreadcrumbItem
            {
                IsActive = isActive,
                DisplayText = publicDivision.GetNameByLanguage(language),
                ActionName = "Show",
                ControllerName = "Division",
                RouteValues = new RouteValueDictionary(new Dictionary<string, object> {
                    { "alias", publicDivision.Alias }
                }),
                HtmlAttributes = null
            };
        }

        public static BreadcrumbItem GetBreadcrumbTimeTableItem()
        {
            return new BreadcrumbItem
            {
                IsActive = true,
                DisplayText = Resources.Resources.Timetable
            };
        }

        public static BreadcrumbItem GetBreadcrumbCourseItem(PublicDivision publicDivision, StudyProgram studyProgram, bool isActive)
        {
            var language = CultureHelper.CurrentLanguage;
            return new BreadcrumbItem
            {
                IsActive = isActive,
                DisplayText = GetCourseItemDisplayText(language, studyProgram),
                ActionName = "Show",
                ControllerName = "StudyProgram",
                RouteValues = new RouteValueDictionary(new Dictionary<string, object> {
                    { "publicDivisionAlias", publicDivision.Alias },
                    { "id", studyProgram.Id }
                }),
                HtmlAttributes = null
            };
        }

        private static string GetCourseItemDisplayText(LanguageCode language, StudyProgram studyProgram)
        {
            return String.Format("{0} \"{1}\"", Resources.Resources.Program, studyProgram.GetNameByLanguage(language));
        }
    }
}