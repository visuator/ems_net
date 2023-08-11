using Ems.Models.Excel;

namespace Ems.Domain.Services;

public interface IClassroomService
{
    Task Import(List<ExcelClassroomModel> models, CancellationToken token = default);
}