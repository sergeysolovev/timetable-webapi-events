using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Program's Student Groups Data Contract
    /// </summary>
    public class ProgramGroupsContract
    {
        /// <summary>
        /// Program's Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Groups
        /// </summary>
        public IEnumerable<Group> Groups { get; set; }

        /// <summary>
        /// Program Group Data Contract
        /// </summary>
        public class Group
        {
            /// <summary>
            /// Id
            /// </summary>
            [JsonProperty("StudentGroupId")]
            public int Id { get; set; }

            /// <summary>
            /// Name
            /// </summary>
            [JsonProperty("StudentGroupName")]
            public string Name { get; set; }

            /// <summary>
            /// Study Form
            /// </summary>
            [JsonProperty("StudentGroupStudyForm")]
            public string Form { get; set; }

            /// <summary>
            /// Profiles
            /// </summary>
            [JsonProperty("StudentGroupProfiles")]
            public string Profiles { get; set; }

            /// <summary>
            /// Division's Alias
            /// </summary>
            [JsonProperty("PublicDivisionAlias", NullValueHandling = NullValueHandling.Ignore)]
            public string DivisionAlias { get; set; }
        }
    }
}

