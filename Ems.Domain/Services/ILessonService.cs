using Ems.Models.Excel;

namespace Ems.Domain.Services;

public interface ILessonService
{
    Task Import(List<ExcelLessonModel> models, CancellationToken token = new());
}