using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Services;
using Ems.Models.Excel;
using ExcelDataReader;

namespace Ems.Domain.Services.Import;

public class ExcelClassVersionImportService : IImportService
{
    private readonly IClassVersionService _classVersionService;

    public ExcelClassVersionImportService(IClassVersionService classVersionService)
    {
        _classVersionService = classVersionService;
    }

    private static Dictionary<string, Quarter> _quarterConverter => new()
    {
        { "1-я неделя", Quarter.First },
        { "2-я неделя", Quarter.Second }
    };

    private static Dictionary<string, DayOfWeek> _dayOfWeekConverter => new()
    {
        { "Пн", DayOfWeek.Monday },
        { "Вт", DayOfWeek.Tuesday },
        { "Ср", DayOfWeek.Wednesday },
        { "Чт", DayOfWeek.Thursday },
        { "Пт", DayOfWeek.Friday },
        { "Сб", DayOfWeek.Saturday },
        { "Вс", DayOfWeek.Sunday }
    };

    private static Dictionary<string, ClassType> _classTypeConverter => new()
    {
        { "Лекция", ClassType.Lecture },
        { "Практика", ClassType.Seminar }
    };

    public async Task Import(Stream file, CancellationToken token = new())
    {
        var classVersionModel = ReadClassVersion(file);
        await _classVersionService.Import(classVersionModel, token);
    }

    private static ExcelClassVersionModel ReadClassVersion(Stream file)
    {
        var reader = ExcelReaderFactory.CreateOpenXmlReader(file);
        var classVersionModel = new ExcelClassVersionModel { Classes = new List<ExcelClassModel>() };

        reader.Read();
        var classVersionName = reader.GetString(1);
        classVersionModel.Name = classVersionName;
        reader.Read();

        while (reader.Read())
        {
            var quarter = _quarterConverter[reader.GetString(0).Trim()];
            var day = _dayOfWeekConverter[reader.GetString(1).Trim()];
            var groupName = reader.GetString(2).Trim();
            var classPeriodName = reader.GetString(3).Trim();
            var lecturerFullName = reader.GetString(4).Trim();
            var lessonName = reader.GetString(5).Trim();
            var classroomName = reader.GetString(6).Trim();
            var type = _classTypeConverter[reader.GetString(7).Trim()];

            classVersionModel.Classes.Add(new ExcelClassModel
            {
                Quarter = quarter,
                Day = day,
                GroupName = groupName,
                ClassPeriodName = classPeriodName,
                LecturerFullName = lecturerFullName,
                LessonName = lessonName,
                ClassroomName = classroomName,
                Type = type
            });
        }

        return classVersionModel;
    }
}