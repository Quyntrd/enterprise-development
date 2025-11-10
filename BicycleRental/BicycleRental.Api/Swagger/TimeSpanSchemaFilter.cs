using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BicycleRental.Api.Swagger;

/// <summary>
/// Adds example and format information for TimeSpan properties in Swagger.
/// </summary>
public class TimeSpanSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(TimeSpan) || context.Type == typeof(TimeSpan?))
        {
            schema.Type = "string";
            schema.Format = "time-span";
            schema.Example = new OpenApiString("03:00:00"); // Example in hh:mm:ss format
        }
    }
}
