using Ems.Infrastructure.Services;
using Ems.Models.Excel;
using ExcelDataReader;

namespace Ems.Domain.Services.Import;

public class ExcelClassroomImportService : IImportService
{
    private readonly IClassroomService _classroomService;

    public ExcelClassroomImportService(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    public async Task Import(Stream file, CancellationToken token = new())
    {
        var classrooms = ReadClassrooms(file);
        await _classroomService.Import(classrooms, token);
    }

    private static List<ExcelClassroomModel> ReadClassrooms(Stream file)
    {
        var reader = ExcelReaderFactory.CreateOpenXmlReader(file);
        var classrooms = new List<ExcelClassroomModel>();

        reader.Read();
        while (reader.Read())
        {
            var name = reader.GetString(0).Trim();

            classrooms.Add(new ExcelClassroomModel
            {
                Name = name
            });
        }

        return classrooms;
    }
}