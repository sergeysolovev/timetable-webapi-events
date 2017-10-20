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
    public class XtracurEventsIndexMonthViewModel : XtracurEventsIndexViewModelBase
    {
        public override string ViewName
        {
            get { return "IndexMonth"; }
        }

        public string Alias { get; private set; }
        public string Title { get; private set; }
        public string ChosenMonthDisplayText { get; private set; }
        public string PreviousMonthDisplayText { get; private set; }
        public string PreviousMonthDate { get; private set; }
        public string NextMonthDisplayText { get; private set; }
        public string NextMonthDate { get; private set; }
        public bool IsCurrentMonthReferenceAvailable { get; private set; }
        public bool ShowGroupingCaptions { get; private set; }
        public bool HasEventsToShow { get; private set; }

        public IEnumerable<XtracurEventGroupingItemViewModel> EventGroupings { get; private set; }

        public static XtracurEventsIndexMonthViewModel Build(XtracurDivision xtracurDivision, DateTime? month,
            int? showImmediateEventId, int? showImmediateRecurrenceIndex)
        {
            var language = CultureHelper.CurrentLanguage;
            var firstDayOfCurrentMonth = DateTimeHelper.GetFirstDayOfCurrentMonth();
            var firstDayOfChosenMonth = DateTimeHelper.GetFirstDayOfMonth(month);
            var firstDayOfTheNextMonth = DateTimeHelper.GetFirstDayOfTheNextMonth(firstDayOfChosenMonth);
            var firstDayOfThePreviousMonth = DateTimeHelper.GetFirstDayOfThePreviousMonth(firstDayOfChosenMonth);
            var eventGroupings = EventsViewModelHelper.GetXtracurEventGroupings(xtracurDivision,
                firstDayOfChosenMonth, firstDayOfTheNextMonth, showImmediateEventId, showImmediateRecurrenceIndex);

            return new XtracurEventsIndexMonthViewModel
            {
                Alias = xtracurDivision.Alias,
                Title = xtracurDivision.GetNameByLanguage(language),
                IsCurrentMonthReferenceAvailable = firstDayOfCurrentMonth != firstDayOfChosenMonth,
                ChosenMonthDisplayText = GetMonthDisplayText(firstDayOfChosenMonth),
                PreviousMonthDisplayText = GetPreviousMonthDisplayText(firstDayOfThePreviousMonth),
                PreviousMonthDate = DateTimeHelper.GetDateStringForWeb(firstDayOfThePreviousMonth),
                NextMonthDisplayText = GetNextMonthDisplayText(firstDayOfTheNextMonth),
                NextMonthDate = DateTimeHelper.GetDateStringForWeb(firstDayOfTheNextMonth),
                EventGroupings = eventGroupings,
                ShowGroupingCaptions = eventGroupings.Count() > 1,
                HasEventsToShow = eventGroupings.Any(eg => eg.Events.Any(e => !e.IsShowImmediateHidden)),
                Breadcrumb = new Breadcrumb()
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(false),
                    BreadcrumbHelper.GetBreadcrumbXtracurEventsItem(xtracurDivision, true)
                }
            };
        }

        private static string GetMonthDisplayText(DateTime month)
        {
            return month.ToString("MMMM yyyy");
        }

        private static string GetPreviousMonthDisplayText(DateTime month)
        {
            return String.Format("« {0}", GetMonthDisplayText(month));
        }

        private static string GetNextMonthDisplayText(DateTime month)
        {
            return String.Format("{0} »", GetMonthDisplayText(month));
        }
    }
}
