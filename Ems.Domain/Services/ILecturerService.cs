using Ems.Models.Excel;

namespace Ems.Domain.Services;

public interface ILecturerService
{
    Task Import(List<ExcelLecturerModel> models, CancellationToken token = new());
}