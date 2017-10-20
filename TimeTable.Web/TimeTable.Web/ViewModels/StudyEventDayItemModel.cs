using SpbuEducation.TimeTable.Common.Web.ViewModels;
using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudyEventDayItemModel
    {
        public DateTime Day { get; private set; }
        public string DayString { get; private set; }
        public IEnumerable<StudyEventItemViewModel> DayStudyEvents { get; private set; }

        public static StudyEventDayItemModel Build(DateTime day, IEnumerable<StudyEventItemViewModel> dayStudyEvents)
        {
            return new StudyEventDayItemModel
            {
                Day = day,
                DayStudyEvents = dayStudyEvents,
                DayString = GetDayString(day)
            };
        }

        public static string GetDayString(DateTime day)
        {
            return day.ToString("dddd");
        }
    }
}