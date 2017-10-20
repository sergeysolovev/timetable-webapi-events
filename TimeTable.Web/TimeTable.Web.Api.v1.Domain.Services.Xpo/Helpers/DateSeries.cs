using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpbuEducation.TimeTable.Helpers.Multilingual;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Helpers
{
    internal class DateSeries : IEnumerable<DateSeriesItem>
    {
        private readonly IEnumerable<DateSeriesItem> source;
        private readonly LanguageCode language;

        #region IEnumerable
        public IEnumerator<DateSeriesItem> GetEnumerator() => source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();
        #endregion

        public DateSeries(IEnumerable<DateTime> dates, LanguageCode languageCode)
        {
            language = languageCode;
            source = FromDates(dates ?? throw new ArgumentNullException(nameof(dates)));
        }

        private IEnumerable<DateSeriesItem> FromDates(IEnumerable<DateTime> dates)
        {
            List<DateSeriesItem> ans = new List<DateSeriesItem>();
            IEnumerable<DateTime> sorted = dates.Distinct().OrderBy(d => d);
            DateTime prev = DateTime.MinValue, from = DateTime.MinValue, to = DateTime.MinValue;
            foreach (DateTime dt in sorted)
            {
                if ((int)(dt - prev).TotalDays != 7)
                {
                    if (from != DateTime.MinValue)
                    {
                        ans.Add(new DateSeriesItem(from, to == DateTime.MinValue ? from : to, language));
                    }
                    from = dt;
                    to = DateTime.MinValue;
                }
                else
                {
                    to = dt;
                }
                prev = dt;
            }
            if (from != DateTime.MinValue)
            {
                ans.Add(new DateSeriesItem(from, to == DateTime.MinValue ? from : to, language));
            }
            return ans;
        }
    }
}
