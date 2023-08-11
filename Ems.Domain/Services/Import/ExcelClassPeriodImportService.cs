using Ems.Infrastructure.Services;
using Ems.Models.Excel;
using ExcelDataReader;

namespace Ems.Domain.Services.Import;

public class ExcelClassPeriodImportService : IImportService
{
    private readonly IClassPeriodService _classPeriodService;

    public ExcelClassPeriodImportService(IClassPeriodService classPeriodService)
    {
        _classPeriodService = classPeriodService;
    }

    public async Task Import(Stream file, CancellationToken token = new())
    {
        var classPeriods = ReadClassPeriods(file);
        await _classPeriodService.Import(classPeriods, token);
    }

    private static List<ExcelClassPeriodModel> ReadClassPeriods(Stream file)
    {
        var reader = ExcelReaderFactory.CreateOpenXmlReader(file);
        var classPeriods = new List<ExcelClassPeriodModel>();

        reader.Read();
        while (reader.Read())
        {
            var startingAt = reader.GetDateTime(0).TimeOfDay;
            var endingAt = reader.GetDateTime(1).TimeOfDay;
            var name = reader.GetString(2).Trim();

            classPeriods.Add(new ExcelClassPeriodModel
            {
                StartingAt = startingAt,
                EndingAt = endingAt,
                Name = name
            });
        }

        return classPeriods;
    }
}