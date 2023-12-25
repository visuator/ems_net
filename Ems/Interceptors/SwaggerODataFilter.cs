using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ems.Interceptors;

public sealed class SwaggerODataFilter : IOperationFilter
{
    private static readonly List<OpenApiParameter> Parameters = new Dictionary<string, string>
    {
        { "$top", "Максимальное количество записей" },
        { "$skip", "Количество записей, необходимое пропустить" },
        { "$filter", "Фильтр" },
        { "$orderby", "Сортировка" },
        { "$expand", "Связанные сущности" }
    }.Select(pair => new OpenApiParameter
    {
        Name = pair.Key,
        Required = false,
        Schema = new OpenApiSchema { Type = "String" },
        In = ParameterLocation.Query,
        Description = pair.Value
    }).ToList();

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
        foreach (var item in Parameters)
            operation.Parameters.Add(item);
    }
}