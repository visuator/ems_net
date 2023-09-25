using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ems.Interceptors;

public class SwaggerJsonIgnoreFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var types = context.ApiDescription.ActionDescriptor.Parameters.SelectMany(x => x.ParameterType.GetMembers())
            .Where(mi => mi.GetCustomAttribute<JsonIgnoreAttribute>() is not null).ToList();
        foreach (var mi in types)
        {
            var parameters = operation.Parameters.Where(x => x.Name.StartsWith(mi.Name)).ToList();
            foreach (var p in parameters)
                operation.Parameters.Remove(p);
        }
    }
}