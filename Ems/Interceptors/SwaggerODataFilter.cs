using Ems.Constants;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ems.Interceptors;

public sealed class SwaggerODataFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var queryParameter =
            context.ApiDescription.ActionDescriptor.Parameters.SingleOrDefault(x =>
                x.ParameterType.IsGenericType &&
                x.ParameterType.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>));
        if (queryParameter is null) return;

        var apiParameter = operation.Parameters.SingleOrDefault(x => x.Name == queryParameter.Name);
        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Remove(apiParameter);
        foreach (var item in SwaggerConstants.Parameters)
            operation.Parameters.Add(item);
    }
}