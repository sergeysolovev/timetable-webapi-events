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
    public class XtracurEventsSearchViewModel : BreadcrumbViewModel
    {
        public const int ItemsOnPageCount = 10;
        public const int PagerNavigationDistance = 2;
        
        public string Alias { get; private set; }
        public string Title { get; private set; }
        public string QueryDisplayText { get; private set; }
        public int Offset { get; private set; }
        public IEnumerable<XtracurEventItemViewModel> Events { get; private set; }
        public IEnumerable<XtracurEventsSearchPagerItemViewModel> PagerItems { get; private set; }
        public bool ShowPager { get; private set; }
        public bool ShowPast { get; private set; }
        public bool ShowUpcoming { get; private set; }
        public bool IsEmptyQuery { get; private set; }

        public static XtracurEventsSearchViewModel Build(XtracurDivision xtracurDivision, string query, int? offset, bool? showPast)
        {
            LanguageCode language = CultureHelper.CurrentLanguage;
            bool isEmptyQuery = String.IsNullOrWhiteSpace(query);
            int totalCount;
            var today = DateTime.Today;
            IEnumerable<XtracurEventItemViewModel> events = Enumerable.Empty<XtracurEventItemViewModel>();
            
            if (!showPast.HasValue && !offset.HasValue)
            {
                var upcomingEvents = GetEvents(xtracurDivision, query, 0, today, DateTimeHelper.MaxValue, out totalCount);
                if (upcomingEvents.Any())
                {
                    events = upcomingEvents;
                    showPast = false;
                }
                else
                {
                    var pastEvents = GetEvents(xtracurDivision, query, 0, DateTimeHelper.MinValue, today.AddDays(1), out totalCount);
                    if (pastEvents.Any())
                    {
                        events = pastEvents;
                        showPast = true;
                    }
                }
            }
            else
            {
                var fromDate = showPast.Value ? DateTimeHelper.MinValue : today;
                var toDate = showPast.Value ? today.AddDays(1) : DateTimeHelper.MaxValue;
                events = GetEvents(xtracurDivision, query, offset ?? 0, fromDate, toDate, out totalCount);
            }
            IEnumerable<XtracurEventsSearchPagerItemViewModel> pagerItems = GeneratePagerItems(offset ?? 0, (totalCount - 1) / ItemsOnPageCount + 1);
            return new XtracurEventsSearchViewModel
            {
                Alias = xtracurDivision.Alias,
                Title = xtracurDivision.GetNameByLanguage(language),
                Events = events,
                PagerItems = pagerItems,
                ShowPager = pagerItems.Any(pi => pi.IsEnabled && !pi.IsActive),
                ShowPast = showPast ?? false,
                ShowUpcoming = showPast.HasValue ? !(showPast ?? false) : false,
                IsEmptyQuery = isEmptyQuery,
                Offset = offset ?? 0,
                QueryDisplayText = query,
                Breadcrumb = new Breadcrumb()
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(false),
                    BreadcrumbHelper.GetBreadcrumbXtracurEventsItem(xtracurDivision, false),
                    BreadcrumbHelper.GetBreadcrumbXtracurSearchItem(true)
                }
            };
        }

        private static IEnumerable<XtracurEventItemViewModel> GetEvents(XtracurDivision xtracurDivision, string query, int offset,
            DateTime fromDate, DateTime toDate, out int totalCount)
        {
            return EventsViewModelHelper.GetXtracurSearchEvents(xtracurDivision, fromDate, toDate, query, offset * ItemsOnPageCount, ItemsOnPageCount, out totalCount);
        }

        private static XtracurEventsSearchPagerItemViewModel[] GeneratePagerItems(int activeOffset, int offsetCount)
        {
            List<int> offsets = new List<int>();
            if (0 <= activeOffset && activeOffset < offsetCount) offsets.Add(activeOffset);
            for (int i = 1; i <= PagerNavigationDistance; i++)
            {
                if (0 <= activeOffset + i && activeOffset + i < offsetCount) offsets.Add(activeOffset + i);
                if (0 <= activeOffset - i && activeOffset - i < offsetCount) offsets.Add(activeOffset - i);
            }
            if (!offsets.Contains(0)) offsets.Add(0);
            if (!offsets.Contains(offsetCount - 1)) offsets.Add(offsetCount - 1);
            offsets.Sort();
            List<XtracurEventsSearchPagerItemViewModel> items = new List<XtracurEventsSearchPagerItemViewModel>();
            int prev = 0;
            items.Add(XtracurEventsSearchPagerItemViewModel.Build("«", activeOffset - 1, activeOffset > 0, false));
            foreach (int offset in offsets)
            {
                if (offset - prev > 2)
                {
                    items.Add(XtracurEventsSearchPagerItemViewModel.Build("…", 0, false, false));
                }
                else if (offset - prev == 2)
                {
                    items.Add(XtracurEventsSearchPagerItemViewModel.Build(offset.ToString(), offset - 1, true, false));
                }
                items.Add(XtracurEventsSearchPagerItemViewModel.Build((offset + 1).ToString(), offset, true, offset == activeOffset));
                prev = offset;
            }
            items.Add(XtracurEventsSearchPagerItemViewModel.Build("»", activeOffset + 1, activeOffset < offsets.Last(), false));
            return items.ToArray();
        }
    }
}
