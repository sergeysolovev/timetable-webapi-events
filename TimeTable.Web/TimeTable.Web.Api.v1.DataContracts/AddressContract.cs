using Newtonsoft.Json;
using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Address Data Contract
    /// </summary>
    public class AddressContract
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
        /// Number of the address classrooms that match
        /// the request criteria
        /// </summary>
        [JsonProperty(PropertyName = "matches")]
        public int Matches { get; set; }

        /// <summary>
        /// Requested equipment
        /// </summary>
        [JsonProperty(PropertyName = "wantingEquipment")]
        public string Equipment { get; set; }
    }
}