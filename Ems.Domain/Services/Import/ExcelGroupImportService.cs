using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Services;
using Ems.Models.Excel;
using ExcelDataReader;

namespace Ems.Domain.Services.Import;

public class ExcelGroupImportService : IImportService
{
    private readonly IGroupService _groupService;

    public ExcelGroupImportService(IGroupService groupService)
    {
        _groupService = groupService;
    }

    private static Dictionary<string, Course> CourseConverter => new()
    {
        { "1-й курс", Course.First },
        { "2-й курс", Course.Second },
        { "3-й курс", Course.Third },
        { "4-й курс", Course.Fourth },
        { "5-й курс", Course.Fifth }
    };

    public async Task Import(Stream file, DateTime? requestedAt = default, CancellationToken token = new())
    {
        var groups = ReadGroups(file);
        await _groupService.Import(groups, token);
    }

    private static List<ExcelGroupModel> ReadGroups(Stream file)
    {
        var reader = ExcelReaderFactory.CreateOpenXmlReader(file);
        var groups = new List<ExcelGroupModel>();

        reader.Read();
        while (reader.Read())
        {
            var name = reader.GetString(0).Trim();
            var courseName = reader.GetString(1).Trim();

            groups.Add(new ExcelGroupModel { Name = name, Course = CourseConverter[courseName] });
        }

        return groups;
    }
}