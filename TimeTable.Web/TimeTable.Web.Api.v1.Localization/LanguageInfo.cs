using SpbuEducation.TimeTable.Helpers.Multilingual;
using System;
using System.Globalization;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization
{
    public class LocaleInfo
    {
        public LanguageCode Language { get; private set; }
        public CultureInfo Culture { get; private set; }

        public LocaleInfo(CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            Culture = culture;
            Language = LanguageHelper.GetLanguageByCulture(culture);
        }
    }
}