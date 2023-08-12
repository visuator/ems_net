using Ems.Infrastructure.Services;
using Ems.Models.Excel;
using ExcelDataReader;

namespace Ems.Domain.Services.Import;

public class ExcelLessonImportService : IImportService
{
    private readonly ILessonService _lessonService;

    public ExcelLessonImportService(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    public async Task Import(Stream file, DateTime? requestedAt = default, CancellationToken token = new())
    {
        var lessons = ReadLessons(file);
        await _lessonService.Import(lessons, token);
    }

    private static List<ExcelLessonModel> ReadLessons(Stream file)
    {
        var reader = ExcelReaderFactory.CreateOpenXmlReader(file);
        var lessons = new List<ExcelLessonModel>();

        reader.Read();
        while (reader.Read())
        {
            var name = reader.GetString(0).Trim();
            lessons.Add(new ExcelLessonModel
            {
                Name = name
            });
        }

        return lessons;
    }
}