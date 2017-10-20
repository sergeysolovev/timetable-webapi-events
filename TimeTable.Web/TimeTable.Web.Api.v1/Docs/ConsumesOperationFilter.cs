using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http.Description;

namespace SpbuEducation.TimeTable.Web.Api.v1.Docs
{
    /// <summary>
    /// Swagger Consumes Operation Filter
    /// </summary>
    public sealed class ConsumesOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply the filter to an operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(
            Operation operation,
            SchemaRegistry schemaRegistry,
            ApiDescription apiDescription)
        {
            var attribute = apiDescription
                .GetControllerAndActionAttributes<SwaggerConsumesAttribute>()
                .SingleOrDefault();

            if (attribute != null)
            {
                operation.consumes.Clear();
                operation.consumes = attribute.ContentTypes.ToList();
            }
        }
    }
}