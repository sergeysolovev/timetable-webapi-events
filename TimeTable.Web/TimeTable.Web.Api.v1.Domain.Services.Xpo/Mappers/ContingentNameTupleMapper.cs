using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers
{
    internal class ContingentNameTupleMapper
    {
        private readonly LanguageCode language;
        private readonly ContingentDivisionCourseMapper divCourseMapper;

        public ContingentNameTupleMapper(
            ContingentDivisionCourseMapper divCourseMapper,
            LocaleInfo locale)
        {
            this.divCourseMapper = divCourseMapper ??
                throw new ArgumentNullException(nameof(divCourseMapper));

            this.language = locale?.Language ??
                throw new ArgumentNullException(nameof(locale));
        }

        public Tuple<string, string> Map(ContingentUnit contingentUnit)
        {
            return new Tuple<string, string>(
                contingentUnit?.Name ?? string.Empty,
                divCourseMapper.Map(contingentUnit)
            );
        }
    }
}