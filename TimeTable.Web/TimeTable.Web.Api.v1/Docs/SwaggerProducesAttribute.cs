using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.Docs
{
    /// <summary>
    /// Swagger Produces Attributes
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SwaggerProducesAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentTypes"></param>
        public SwaggerProducesAttribute(params string[] contentTypes)
        {
            ContentTypes = contentTypes;
        }

        /// <summary>
        /// Content types
        /// </summary>
        public IList<string> ContentTypes { get; }
    }
}