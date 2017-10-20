using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Educators Data Contract
    /// </summary>
    public class EducatorsContract
    {
        /// <summary>
        /// Last Name Search Query
        /// </summary>
        public string EducatorLastNameQuery { get; set; }

        /// <summary>
        /// Educators
        /// </summary>
        public IEnumerable<EducatorContract> Educators { get; set; }
    }
}
