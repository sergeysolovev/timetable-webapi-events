﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using System.Collections;
using SpbuEducation.TimeTable.Web.Models;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Web.Helpers;
using System.Globalization;
using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using SpbuEducation.TimeTable.Appointments.Spreadsheets;
using SpbuEducation.TimeTable.Helpers.Multilingual;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class EducatorEventsShowViewModel : BreadcrumbViewModel
    {
        public string EducatorDisplayText { get; private set; }
        public string Title { get; private set; }
        public int EducatorMasterId { get; private set; }
        public string PreviousWeekMonday { get; private set; }
        public string WeekMonday { get; private set; }
        public string NextWeekMonday { get; private set; }
        public string WeekDisplayText { get; private set; }
        public bool IsCurrentWeekReferenceAvailable { get; private set; }
        public IEnumerable<StudyEventDayItemViewModel> EducatorEventsDays { get; private set; }

        public static EducatorEventsShowViewModel Build(EducatorMasterPerson educatorMasterPerson, DateTime? weekMonday)
        {
            LanguageCode language = CultureHelper.CurrentLanguage;
            DateTime weekStart = weekMonday.HasValue
                ? weekMonday.Value.AddDays(1 - (int)weekMonday.Value.DayOfWeek).Date
                : DateTime.Now.AddDays(1 - (int)DateTime.Now.DayOfWeek).Date;
            DateTime weekEnd = weekStart.AddDays(7);
            string educatorDisplayText = educatorMasterPerson.GetDisplayNameByLanguage(language);
            string educatorFullDisplayText = educatorMasterPerson.GetLongDisplayNameByLanguage(language);
            return new EducatorEventsShowViewModel
            {
                EducatorDisplayText = educatorDisplayText,
                EducatorFullDisplayText = educatorFullDisplayText,
                Title = string.Format("{0} {1}", Resources.Resources.TimetableForEducator, educatorFullDisplayText),
                EducatorMasterId = educatorMasterPerson.Id,
                EducatorEventsDays = StudyEventsViewModelHelper.GetEducatorEventsDayViewModels(educatorMasterPerson, weekStart, weekEnd),
                PreviousWeekMonday = weekStart.AddDays(-7).ToString("yyyy-MM-dd"),
                WeekStart = weekStart,
                WeekMonday = weekStart.ToString("yyyy-MM-dd"),
                NextWeekMonday = weekStart.AddDays(7).ToString("yyyy-MM-dd"),
                WeekDisplayText = language == LanguageCode.English
                    ? string.Format("{0:d MMMM} – {1:d MMMM}", weekStart, weekEnd.AddDays(-1))
                    : string.Format("{0:d MMMM} – {1:d MMMM}", weekStart, weekEnd.AddDays(-1)),
                IsCurrentWeekReferenceAvailable = weekStart != DateTime.Now.AddDays(1 - (int)DateTime.Now.DayOfWeek).Date,
                Breadcrumb = new Breadcrumb()
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(false),
                    BreadcrumbHelper.GetBreadcrumbEducatorsItem(false),
                    new BreadcrumbItem
                    {
                        IsActive = true,
                        DisplayText = educatorDisplayText,
                    }
                }
            };
        }

        public DateTime WeekStart { get; set; }

        public string EducatorFullDisplayText { get; set; }
    }
}
