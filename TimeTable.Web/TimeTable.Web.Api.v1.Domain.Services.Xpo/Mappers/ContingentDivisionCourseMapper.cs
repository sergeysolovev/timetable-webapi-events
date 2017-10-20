using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using System;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers
{
    internal class ContingentDivisionCourseMapper
    {
        private readonly LanguageCode language;

        public ContingentDivisionCourseMapper(LocaleInfo locale)
        {
            language = locale?.Language ??
                throw new ArgumentNullException(nameof(locale));
        }

        public string Map(ContingentUnit contingentUnit)
        {
            if (contingentUnit != null)
            {
                var division = contingentUnit?.Division?.PublicDivisions?.FirstOrDefault();
                var course = (language == LanguageCode.English) ?
                    $"{contingentUnit.CourseNumber} course" :
                    $"{contingentUnit.CourseNumber} курс";
                var divisionName = division?.GetNameByLanguage(language);

                return (division?.GetNameByLanguage(language) == null) ?
                    course : $"{divisionName} — {course}";
            }

            return string.Empty;
        }
    }
}