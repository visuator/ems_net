using Ems.Models.Excel;

namespace Ems.Domain.Services;

public interface IStudentService
{
    Task Import(DateTime requestedAt, List<ExcelStudentModel> models, CancellationToken token = new());
    Task<bool> Exists(Guid id, CancellationToken token = new());
}