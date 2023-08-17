using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface IClassroomService
{
    Task Import(List<ExcelClassroomModel> models, CancellationToken token = new());
    Task<List<ClassroomDto>> GetAll(ODataQueryOptions<ClassroomDto> query, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
}