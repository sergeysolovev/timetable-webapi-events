using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using System;
using System.Globalization;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers
{
    internal class EventLocationMapper
    {
        private readonly LanguageCode language;
        private readonly EducatorIdTupleMapper educatorIdMapper;

        public EventLocationMapper(
            EducatorIdTupleMapper educatorIdMapper,
            LocaleInfo locale)
        {
            this.educatorIdMapper = educatorIdMapper ??
                throw new ArgumentNullException(nameof(educatorIdMapper));

            this.language = locale?.Language ??
                throw new ArgumentNullException(nameof(locale));
        }

        public EventLocationContract Map(EventLocation eventLocation)
        {
            var contract = new EventLocationContract
            {
                IsEmpty = (eventLocation == null),
                DisplayName = string.Empty,
                LatitudeValue = string.Empty,
                LongitudeValue = string.Empty
            };
            
            if (eventLocation != null)
            {
                contract.DisplayName = GetLocationDisplayName(eventLocation);

                if (eventLocation.Location != null && eventLocation.Location.HasGeographicCoordinates)
                {
                    contract.HasGeographicCoordinates = true;
                    contract.Latitude = eventLocation.Location.Latitude;
                    contract.Longitude = eventLocation.Location.Longitude;
                    contract.LatitudeValue = RenderGeoCoordinateValue(contract.Latitude);
                    contract.LongitudeValue = RenderGeoCoordinateValue(contract.Longitude);
                }

                var educatorsDisplayText = string.Join(", ", eventLocation
                    .Educators
                    .Select(sele => sele.EducatorEmployment != null
                        ? sele.EducatorEmployment.GetDisplayNameByLanguage(language)
                        : sele.Educator != null
                            ? sele.Educator.GetDisplayNameByLanguage(language) : "")
                    );

                contract.HasEducators = !string.IsNullOrEmpty(educatorsDisplayText);
                contract.EducatorsDisplayText = educatorsDisplayText;
                contract.EducatorIds = eventLocation.Educators.Select(educatorIdMapper.Map);
            }

            return contract;
        }

        private string GetLocationDisplayName(EventLocation eventLocation)
        {
            string displayName = eventLocation.Location.GetDisplayName2ByLanguage(language);
            if (language == LanguageCode.English && String.IsNullOrEmpty(displayName))
            {
                displayName = eventLocation.Location.GetDisplayName2ByLanguage(LanguageCode.Russian);
            }
            return displayName;
        }

        private string RenderGeoCoordinateValue(double value) => value != 0.0 ?
            value.ToString(new NumberFormatInfo { NumberDecimalSeparator = "." }) :
            string.Empty;
    }
}