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
    public abstract class XtracurEventsIndexViewModelBase : BreadcrumbViewModel
    {
        public abstract string ViewName { get; }

        public static XtracurEventsIndexViewModelBase Build(XtracurDivision xtracurDivision, DateTime? fromDate,
            int? showImmediateEventId, int? showImmediateRecurrenceIndex)
        {
            if (xtracurDivision != null)
            {
                switch (xtracurDivision.WebViewKind)
                {
                    case XtracurEventsWebViewKind.Month:
                        return XtracurEventsIndexMonthViewModel.Build(xtracurDivision, fromDate, showImmediateEventId, showImmediateRecurrenceIndex);
                    case XtracurEventsWebViewKind.Week:
                    default:
                        return XtracurEventsIndexWeekViewModel.Build(xtracurDivision, fromDate, showImmediateEventId, showImmediateRecurrenceIndex);
                }
            }
            return null;
        }
    }
}
