using Ems.Models.Dtos;
using Ems.Models.Excel;
using Microsoft.AspNetCore.OData.Query;

namespace Ems.Domain.Services;

public interface ILecturerService
{
    Task Import(DateTime requestedAt, List<ExcelLecturerModel> models, CancellationToken token = new());
    Task<List<LecturerDto>> GetAll(ODataQueryOptions<LecturerDto> query, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
}