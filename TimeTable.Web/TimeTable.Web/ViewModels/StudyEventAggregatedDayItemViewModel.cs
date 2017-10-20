using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Helpers;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudyEventAggregatedDayItemViewModel
    {
        public DayOfWeek Day { get; private set; }
        public string DayString { get; private set; }
        public IEnumerable<StudyEventAggregatedItemViewModel> DayStudyEvents { get; private set; }
        public int DayStudyEventsCount { get; private set; }

        public static StudyEventAggregatedDayItemViewModel Build(DayOfWeek day, List<StudyEventAggregatedItemViewModel> dayStudyEvents)
        {
            LanguageCode language = CultureHelper.CurrentLanguage;

            return new StudyEventAggregatedDayItemViewModel
            {
                Day = day,
                DayStudyEvents = dayStudyEvents,
                DayString = DateTimeHelper.GetDayOfWeekStringByLanguage(day, language),
                DayStudyEventsCount = dayStudyEvents.Count()
            };
        }
    }
}
