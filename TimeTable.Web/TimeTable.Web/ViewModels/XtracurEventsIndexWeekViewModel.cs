using System;
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
using SpbuEducation.TimeTable.Helpers.Multilingual;
using System.Globalization;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class XtracurEventsIndexWeekViewModel : XtracurEventsIndexViewModelBase
    {
        public override string ViewName
        {
            get { return "IndexWeek"; }
        }
        public bool IsPreviousWeekReferenceAvailable { get { return true; } }
        public bool IsNextWeekReferenceAvailable { get { return true; } }
        public bool IsCurrentWeekReferenceAvailable { get; set; }
        public string PreviousWeekMonday { get; set; }
        public string NextWeekMonday { get; set; }
        public string WeekDisplayText { get; set; }
        public IEnumerable<XtracurEventItemViewModel> EarlierEvents { get; set; }
        public IEnumerable<EventsDayViewModel> Days { get; set; }
        public string Alias { get; private set; }
        public string Title { get; private set; }
        public bool HasEventsToShow { get; private set; }
        public string WeekMonday { get; set; }

        public new static XtracurEventsIndexWeekViewModel Build(XtracurDivision xtracurDivision, DateTime? fromDate,
            int? showImmediateEventId, int? showImmediateRecurrenceIndex)
        {
            var language = CultureHelper.CurrentLanguage;
            var weekFromDate = DateTimeHelper.GetWeekStart(fromDate);
            var weekToDate = weekFromDate.AddDays(7);
            var previousWeekFromDate = weekFromDate.AddDays(-7);
            var nextWeekFromDate = weekFromDate.AddDays(7);
            var currentWeekFromDate = DateTimeHelper.GetWeekStart(DateTime.Today);
            var events = EventsViewModelHelper.GetXtracurEvents(xtracurDivision, weekFromDate, weekToDate, showImmediateEventId, showImmediateRecurrenceIndex).ToList();
            var weekDays = events.Where(e => e.Start >= weekFromDate)
                .GroupBy(vm => vm.Start.Date)
                .OrderBy(g => g.Key)
                .Select(g => EventsDayViewModel.Build(g.Key, g.AsEnumerable()));
            var earlierEvents = events.Where(e => e.Start < weekFromDate);

            return new XtracurEventsIndexWeekViewModel
            {
                Alias = xtracurDivision.Alias,
                Title = xtracurDivision.GetNameByLanguage(language),
                EarlierEvents = earlierEvents,
                Days = weekDays,
                WeekDisplayText = DateTimeHelper.GetWeekDisplayText(language, weekFromDate, weekToDate),
                PreviousWeekMonday = DateTimeHelper.GetDateStringForWeb(previousWeekFromDate),
                WeekMonday = DateTimeHelper.GetDateStringForWeb(weekFromDate),
                NextWeekMonday = DateTimeHelper.GetDateStringForWeb(nextWeekFromDate),
                IsCurrentWeekReferenceAvailable = (currentWeekFromDate != weekFromDate),
                HasEventsToShow = events.Any(e => !e.IsShowImmediateHidden),
                Breadcrumb = new Breadcrumb()
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(false),
                    BreadcrumbHelper.GetBreadcrumbXtracurEventsItem(xtracurDivision, true)
                }
            };
        }
    }
}
