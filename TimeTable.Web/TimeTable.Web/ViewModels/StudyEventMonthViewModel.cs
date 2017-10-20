using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using DevExpress.XtraScheduler;
using SpbuEducation.TimeTable.Appointments;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudyEventMonthViewModel : IViewModel
    {
        public IEnumerable<StudyEventItemViewModel> MonthStudyEvents { get; set; }
        public DateTime FirstDayOfMonth { get; set; }
        public string MonthDisplayText { get; set; }
        public bool IsActive { get; set; }

        public static StudyEventMonthViewModel Build(DateTime firstDayOfMonth, IEnumerable<StudyEventItemViewModel> monthStudyEvents)
        {
            var language = CultureHelper.CurrentLanguage;
            return new StudyEventMonthViewModel
            {
                FirstDayOfMonth = firstDayOfMonth,
                MonthDisplayText = firstDayOfMonth.ToString("MMMM yyyy"),
                MonthStudyEvents = monthStudyEvents
            };
        }
    }
}
