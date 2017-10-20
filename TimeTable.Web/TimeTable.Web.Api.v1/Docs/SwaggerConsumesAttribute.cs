using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.Docs
{
    /// <summary>
    /// Swagger Consumes Attributes
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SwaggerConsumesAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentTypes"></param>
        public SwaggerConsumesAttribute(params string[] contentTypes)
        {
            ContentTypes = contentTypes;
        }

        /// <summary>
        /// Content Types
        /// </summary>
        public IList<string> ContentTypes { get; }
    }
}