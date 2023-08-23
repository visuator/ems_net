using Microsoft.OpenApi.Models;

namespace Ems.Constants;

public static class SwaggerConstants
{
    public static readonly List<OpenApiParameter> Parameters = new Dictionary<string, string>()
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
        Description = pair.Value,
    }).ToList();
}