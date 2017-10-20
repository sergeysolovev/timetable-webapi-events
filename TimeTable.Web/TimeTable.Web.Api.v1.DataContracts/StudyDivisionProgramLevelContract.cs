using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Study Division's Program Level Data Contract
    /// </summary>
    public class StudyDivisionProgramLevelContract
    {
        /// <summary>
        /// Study Level Name
        /// </summary>
        [JsonProperty("StudyLevelName")]
        public string Name { get; set; }

        /// <summary>
        /// English Name (from view model)
        /// </summary>
        [JsonProperty("StudyLevelNameEnglish")]
        public string NameEnglish { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public bool HasCourse6 { get; set; }

        /// <summary>
        /// Study Program Combinations
        /// </summary>
        [JsonProperty("StudyProgramCombinations")]
        public IEnumerable<ProgramCombination> ProgramCombinations { get; set; }

        /// <summary>
        /// Study Program Combination Data Contract
        /// </summary>
        public class ProgramCombination
        {
            /// <summary>
            /// Program's Name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Program's English Name (from view model)
            /// </summary>
            public string NameEnglish { get; set; }

            /// <summary>
            /// Program's admission years
            /// </summary>
            [JsonProperty("AdmissionYears")]
            public IEnumerable<Year> Years { get; set; }
        }

        /// <summary>
        /// Program's Admission Year Data Contract
        /// </summary>
        public class Year
        {
            /// <summary>
            /// Program's Id
            /// </summary>
            [JsonProperty("StudyProgramId")]
            public int ProgramId { get; set; }

            /// <summary>
            /// Program's Name
            /// </summary>
            [JsonProperty("YearName")]
            public string Name { get; set; }

            /// <summary>
            /// Year Number
            /// </summary>
            [JsonProperty("YearNumber")]
            public int Number { get; set; }

            /// <summary>
            /// Determines whether year is empty
            /// </summary>
            public bool IsEmpty { get; set; }

            /// <summary>
            /// Division's alias
            /// </summary>
            [JsonProperty("PublicDivisionAlias")]
            public string DivisionAlias { get; set; }
        }
    }
}
