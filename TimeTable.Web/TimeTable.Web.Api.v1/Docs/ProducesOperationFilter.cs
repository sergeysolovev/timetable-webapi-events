using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http.Description;

namespace SpbuEducation.TimeTable.Web.Api.v1.Docs
{
    /// <summary>
    /// Swagger Produces Operation Filter
    /// </summary>
    public class ProducesOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply the filter to an operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var attribute = apiDescription
                .GetControllerAndActionAttributes<SwaggerProducesAttribute>()
                .SingleOrDefault();

            if (attribute != null)
            {
                operation.produces.Clear();
                operation.produces = attribute.ContentTypes;
            }
        }
    }
}