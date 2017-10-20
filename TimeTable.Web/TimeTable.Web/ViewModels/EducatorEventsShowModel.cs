using System;
using System.Collections.Generic;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Web.Helpers;
using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using SpbuEducation.TimeTable.Helpers.Multilingual;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class EducatorEventsShowModel
    {
        public string EducatorDisplayText { get; private set; }
        public string Title { get; private set; }
        public int EducatorMasterId { get; private set; }
        public string PreviousWeekMonday { get; private set; }
        public string WeekMonday { get; private set; }
        public string NextWeekMonday { get; private set; }
        public string WeekDisplayText { get; private set; }
        public bool IsCurrentWeekReferenceAvailable { get; private set; }
        public IEnumerable<StudyEventDayItemModel> EducatorEventsDays { get; private set; }

        public static EducatorEventsShowModel Build(EducatorMasterPerson educatorMasterPerson, DateTime? weekMonday)
        {
            LanguageCode language = CultureHelper.CurrentLanguage;
            DateTime weekStart = weekMonday.HasValue
                ? weekMonday.Value.AddDays(1 - (int)weekMonday.Value.DayOfWeek).Date
                : DateTime.Now.AddDays(1 - (int)DateTime.Now.DayOfWeek).Date;
            DateTime weekEnd = weekStart.AddDays(7);
            string educatorDisplayText = educatorMasterPerson.GetDisplayNameByLanguage(language);
            string educatorFullDisplayText = educatorMasterPerson.GetLongDisplayNameByLanguage(language);
            return new EducatorEventsShowModel
            {
                EducatorDisplayText = educatorDisplayText,
                EducatorFullDisplayText = educatorFullDisplayText,
                Title = $"{Resources.Resources.TimetableForEducator} {educatorFullDisplayText}",
                EducatorMasterId = educatorMasterPerson.Id,
                EducatorEventsDays = StudyEventsViewModelHelper.GetEducatorEventsDayModels(educatorMasterPerson, weekStart, weekEnd),
                PreviousWeekMonday = weekStart.AddDays(-7).ToString("yyyy-MM-dd"),
                WeekStart = weekStart,
                WeekMonday = weekStart.ToString("yyyy-MM-dd"),
                NextWeekMonday = weekStart.AddDays(7).ToString("yyyy-MM-dd"),
                WeekDisplayText = $"{weekStart:d MMMM} – {weekEnd.AddDays(-1):d MMMM}",
                IsCurrentWeekReferenceAvailable = weekStart != DateTime.Now.AddDays(1 - (int)DateTime.Now.DayOfWeek).Date,
                
            };
        }

        public DateTime WeekStart { get; set; }

        public string EducatorFullDisplayText { get; set; }
    }
}
