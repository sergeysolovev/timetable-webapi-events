using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.Errors
{
    /// <summary>
    /// Custom errors contract
    /// </summary>
    [DataContract]
    [KnownType(typeof(Internal.ErrorsContract))]
    public abstract class ErrorsContract
    {
        /// <summary>
        /// Status Code
        /// </summary>
        [DataMember(IsRequired = true, Name = "statusCode")]
        public string StatusCode { get; set; }

        /// <summary>
        /// Errors
        /// </summary>
        [DataMember(IsRequired = true, Name = "errors")]
        public IList<string> Errors { get; set; }

        /// <summary>
        /// Help url
        /// </summary>
        [DataMember(IsRequired = true, Name = "helpUrl")]
        public string HelpUrl { get; set; }
    }
}