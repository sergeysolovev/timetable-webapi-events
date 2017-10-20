using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Event Location Data Contract
    /// </summary>
    public class EventLocationContract
    {
        /// <summary>
        /// Determines whether event location empty
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Determines whether event location has geo coordinates
        /// </summary>
        public bool HasGeographicCoordinates { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude Value
        /// </summary>
        public string LatitudeValue { get; set; }

        /// <summary>
        /// Longitude Value
        /// </summary>
        public string LongitudeValue { get; set; }

        /// <summary>
        /// Educators Display Text
        /// </summary>
        public string EducatorsDisplayText { get; set; }

        /// <summary>
        /// Determines whether event location contains educators
        /// </summary>
        public bool HasEducators { get; set; }

        /// <summary>
        /// Educators Ids
        /// </summary>
        public IEnumerable<Tuple<int, string>> EducatorIds { get; set; }
    }
}
