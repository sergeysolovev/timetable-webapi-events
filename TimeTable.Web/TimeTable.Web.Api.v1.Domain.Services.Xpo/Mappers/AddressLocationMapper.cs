using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.BusinessObjects.RealEstate;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using System;
using System.Globalization;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers
{
    internal class AddressLocationMapper
    {
        private readonly LanguageCode language;

        public AddressLocationMapper(LocaleInfo locale)
        {
            language = locale?.Language ??
                throw new ArgumentNullException(nameof(locale));
        }

        public AddressLocationContract Map(EventLocation eventLocation, Address address)
        {
            var contract = new AddressLocationContract
            {
                IsEmpty = (eventLocation == null && address == null),
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
            }
            else if (address != null)
            {
                contract.DisplayName = GetAddressDisplayName(address, language);

                if (address.HasGeographicCoordinates)
                {
                    contract.HasGeographicCoordinates = true;
                    contract.Latitude = address.Latitude;
                    contract.Longitude = address.Longitude;
                    contract.LatitudeValue = RenderGeoCoordinateValue(contract.Latitude);
                    contract.LongitudeValue = RenderGeoCoordinateValue(contract.Longitude);
                }
            }

            return contract;
        }

        private string GetLocationDisplayName(EventLocation eventLocation)
        {
            var displayName = eventLocation.Location.GetDisplayName2ByLanguage(language);
            if (language == LanguageCode.English && String.IsNullOrEmpty(displayName))
            {
                displayName = eventLocation.Location.GetDisplayName2ByLanguage(LanguageCode.Russian);
            }
            return displayName;
        }

        private string GetAddressDisplayName(Address address, LanguageCode language)
        {
            var displayName = address.GetDisplayName1ByLanguage(language);
            if (language == LanguageCode.English && String.IsNullOrEmpty(displayName))
            {
                displayName = address.GetDisplayName1ByLanguage(LanguageCode.Russian);
            }
            return displayName;
        }

        private string RenderGeoCoordinateValue(double value) => value != 0.0 ?
            value.ToString(new NumberFormatInfo { NumberDecimalSeparator = "." }) :
            string.Empty;
    }
}