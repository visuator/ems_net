using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface ILessonService
{
    Task Import(List<ExcelLessonModel> models, CancellationToken token = new());
    Task<List<LessonDto>> GetAll(ODataQueryOptions<LessonDto> query, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
}