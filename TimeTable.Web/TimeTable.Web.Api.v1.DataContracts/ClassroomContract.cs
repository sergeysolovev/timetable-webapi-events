using Newtonsoft.Json;
using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Classroom Data Contract
    /// </summary>
    public class ClassroomContract
    {
        /// <summary>
        /// Oid
        /// </summary>
        [JsonProperty(PropertyName = "Oid")]
        public Guid Oid { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        [JsonProperty(PropertyName = "DisplayName1")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Seating Type
        /// </summary>
        [JsonProperty(PropertyName = "SeatingType")]
        public int SeatingType { get; set; }

        /// <summary>
        /// Capacity - number of places
        /// </summary>
        [JsonProperty(PropertyName = "Capacity")]
        public int Capacity { get; set; }

        /// <summary>
        /// Additional Info
        /// </summary>
        [JsonProperty(PropertyName = "AdditionalInfo")]
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Equipment
        /// </summary>
        [JsonProperty(PropertyName = "wantingEquipment")]
        public string Equipment { get; set; }
    }
}