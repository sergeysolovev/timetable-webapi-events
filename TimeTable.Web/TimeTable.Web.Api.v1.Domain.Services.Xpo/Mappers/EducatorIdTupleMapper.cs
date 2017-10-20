using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers
{
    internal class EducatorIdTupleMapper
    {
        private readonly LanguageCode language;

        public EducatorIdTupleMapper(LocaleInfo locale)
        {
            language = locale?.Language ??
                throw new ArgumentNullException(nameof(locale));
        }

        public Tuple<int, string> Map(LocationEducator le)
        {
            return le.EducatorEmployment != null ?
                new Tuple<int, string>(
                    le?.EducatorEmployment?.EducatorPerson?.MasterPerson?.Id ?? -1,
                    le?.EducatorEmployment?.GetDisplayNameByLanguageFormated(language)
                ) :
                le.Educator != null ?
                    new Tuple<int, string>(le.Educator.Id, le.Educator.GetDisplayNameByLanguage(language)) :
                    null;
        }
    }
}