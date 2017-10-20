using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Study Division Data Contract
    /// </summary>
    public class StudyDivisionContract
    {
        /// <summary>
        /// Oid
        /// </summary>
        public Guid Oid { get; set; }

        /// <summary>
        /// Alias - short name code
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }
}
