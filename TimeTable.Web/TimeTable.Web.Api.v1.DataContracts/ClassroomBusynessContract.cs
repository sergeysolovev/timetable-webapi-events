using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Classroom Busyness Data Contract
    /// </summary>
    public class ClassroomBusynessContract
    {
        /// <summary>
        /// Oid
        /// </summary>
        public Guid Oid { get; set; }

        /// <summary>
        /// Checked interval start datetime
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Checked interval end datetime
        /// </summary>
        public DateTime To { get; set; }

        /// <summary>
        /// Determines whether the classroom is busy
        /// in the given interval
        /// </summary>
        public bool IsBusy { get; set; }
    }
}