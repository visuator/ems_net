using Ems.Models.Excel;

namespace Ems.Domain.Services;

public interface IClassPeriodService
{
    Task Import(List<ExcelClassPeriodModel> models, CancellationToken token = new());
}