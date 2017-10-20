using System;
using SpbuEducation.TimeTable.Helpers.Multilingual;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Helpers
{
    internal class DateSeriesItem
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public LanguageCode Language { get; private set; }
        public int Count { get; private set; }

        public DateSeriesItem(DateTime from, DateTime to, LanguageCode language)
        {
            From = from;
            To = to;
            Language = language;
            Count = (int)((to - from).TotalDays / 7) + 1;
        }

        public override string ToString() => (From == To) ?
            From.ToString("d.M") :
            Language == LanguageCode.English ?
                string.Format("from {0:d.M} to {1:d.M} ({2})", From, To, Count) :
                string.Format("с {0:d.M} по {1:d.M} ({2})", From, To, Count);
    }
}