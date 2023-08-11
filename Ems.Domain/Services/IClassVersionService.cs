using Ems.Models;
using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface IClassVersionService
{
    Task Import(ExcelClassVersionModel model, CancellationToken token = new());
    Task Publish(PublishClassVersionModel model, CancellationToken token = new());
    Task<List<ClassVersionDto>> GetAll(ODataQueryOptions<ClassVersionDto> query, CancellationToken token = new());
}