using System;
using System.Collections.Generic;
using System.Linq;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Web.Helpers;
using SpbuEducation.TimeTable.Helpers.Multilingual;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class DateTimeAggregatedViewModel : IDateTimeViewModel
    {
        public IEnumerable<IDateTimeItemViewModel> Dates { get; private set; }
        public IDateTimeItemViewModel Time { get; private set; }

        private DateTimeAggregatedViewModel(IEnumerable<DateTime> dates, TimeSpan start, TimeSpan? end)
        {
            var language = CultureHelper.CurrentLanguage;

            Time = new DateTimeItemViewModel
            {
                ClassName = null,
                Tooltip = Properties.Resources.EventTime,
                Value = GetTimeIntervalStringByLanguage(start, end)
            };

            Dates = DateSeriesHelper
                .Build((dates ?? Enumerable.Empty<DateTime>()))
                .Select(ds => new DateTimeItemViewModel
                {
                    ClassName = null,
                    Tooltip = ds.GetDescriptionByLanguage(language),
                    Value = ds.ToString()
                });
        }

        public static DateTimeAggregatedViewModel Build(IEnumerable<DateTime> dates, TimeSpan start, TimeSpan? end) =>
            new DateTimeAggregatedViewModel(dates, start, end);

        private static string GetTimeIntervalStringByLanguage(TimeSpan start, TimeSpan? end) =>
            $"{new DateTime(start.Ticks):HH:mm}" + (end.HasValue ? $"–{new DateTime(end.Value.Ticks):HH:mm}" : string.Empty);
    }
}