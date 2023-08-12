using Ems.Infrastructure.Services;
using Ems.Models.Excel;
using ExcelDataReader;

namespace Ems.Domain.Services.Import;

public class ExcelLecturerImportService : IImportService
{
    private readonly ILecturerService _lecturerService;

    public ExcelLecturerImportService(ILecturerService lecturerService)
    {
        _lecturerService = lecturerService;
    }

    public async Task Import(Stream file, DateTime? requestedAt = default, CancellationToken token = new())
    {
        var lecturers = ReadLecturers(file);
        await _lecturerService.Import(requestedAt!.Value, lecturers, token);
    }

    private static List<ExcelLecturerModel> ReadLecturers(Stream file)
    {
        var reader = ExcelReaderFactory.CreateOpenXmlReader(file);
        var lecturers = new List<ExcelLecturerModel>();

        reader.Read();
        while (reader.Read())
        {
            var lastName = reader.GetString(0).Trim();
            var firstName = reader.GetString(1).Trim();
            var middleName = reader.GetString(2).Trim();
            var phone = reader.GetString(3).Trim();
            var email = reader.GetString(4).Trim();

            lecturers.Add(new ExcelLecturerModel
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Phone = phone,
                Email = email
            });
        }

        return lecturers;
    }
}