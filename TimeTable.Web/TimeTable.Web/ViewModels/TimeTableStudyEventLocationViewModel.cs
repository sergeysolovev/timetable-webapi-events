using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class TimeTableStudyEventLocationViewModel : IViewModel
    {
        public string LocationName { get; set; }
        public bool HasGeographicCoordinates { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string EducatorsDisplayText { get; set; }
        public bool HasEducators { get; set; }

        public static TimeTableStudyEventLocationViewModel Build(EventLocation eventLocation)
        {
            var language = CultureHelper.CurrentLanguage;
            string educatorsDisplayText = String.Join(", ", eventLocation
                .Educators
                .Select(sele => sele.EducatorEmployment.GetDisplayNameByLanguage(language)));
            return new TimeTableStudyEventLocationViewModel
            {
                LocationName = eventLocation.Location.GetDisplayName2ByLanguage(language),
                HasGeographicCoordinates = eventLocation.Location.HasGeographicCoordinates,
                HasEducators = !String.IsNullOrEmpty(educatorsDisplayText),
                Latitude = eventLocation.Location.Latitude,
                Longitude = eventLocation.Location.Longitude,
                EducatorsDisplayText = educatorsDisplayText
            };
        }
    }
}
