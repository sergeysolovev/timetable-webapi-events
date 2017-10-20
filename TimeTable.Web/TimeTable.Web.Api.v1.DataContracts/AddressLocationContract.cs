namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Address Location Data Contract
    /// </summary>
    public class AddressLocationContract
    {
        /// <summary>
        /// Is Empty
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// Determines whether it has geo coordinates
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
        /// Latitude value
        /// </summary>
        public string LatitudeValue { get; set; }

        /// <summary>
        /// Longitude value
        /// </summary>
        public string LongitudeValue { get; set; }
    }
}