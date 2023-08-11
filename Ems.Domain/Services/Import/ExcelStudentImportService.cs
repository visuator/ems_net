using Ems.Infrastructure.Services;
using Ems.Models.Excel;
using ExcelDataReader;

namespace Ems.Domain.Services.Import;

public class ExcelStudentImportService : IImportService
{
    private readonly IStudentService _studentService;

    public ExcelStudentImportService(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task Import(Stream file, CancellationToken token = new())
    {
        var students = ReadStudents(file);
        await _studentService.Import(students, token);
    }

    private static List<ExcelStudentModel> ReadStudents(Stream file)
    {
        var students = new List<ExcelStudentModel>();
        var reader = ExcelReaderFactory.CreateOpenXmlReader(file);

        reader.Read();
        while (reader.Read())
        {
            var lastName = reader.GetString(0).Trim();
            var firstName = reader.GetString(1).Trim();
            var middleName = reader.GetString(2).Trim();
            var phone = reader.GetString(3).Trim();
            var email = reader.GetString(4).Trim();
            var groupName = reader.GetString(5).Trim();

            students.Add(new ExcelStudentModel
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Phone = phone,
                Email = email,
                GroupName = groupName
            });
        }

        return students;
    }
}