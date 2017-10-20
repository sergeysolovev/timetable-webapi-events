using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudyEventDayItemViewModel
    {
        public DateTime Day { get; private set; }
        public string DayString { get; private set; }
        public IEnumerable<StudyEventItemViewModel> DayStudyEvents { get; private set; }

        public static StudyEventDayItemViewModel Build(DateTime day, IEnumerable<StudyEventItemViewModel> dayStudyEvents)
        {
            return new StudyEventDayItemViewModel
            {
                Day = day,
                DayStudyEvents = dayStudyEvents,
                DayString = GetDayString(day)
            };
        }

        public static string GetDayString(DateTime day)
        {
            var language = CultureHelper.CurrentLanguage;
            if (language == LanguageCode.English)
            {
                return day.ToString("dddd, MMMM d");
            }
            else
            {
                return day.ToString("dddd, d MMMM");
            }
        }
    }
}