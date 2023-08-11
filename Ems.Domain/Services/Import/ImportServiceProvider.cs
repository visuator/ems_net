using Ems.Domain.Enums;
using Ems.Infrastructure.Enums;
using Ems.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ems.Domain.Services.Import;

public class ImportServiceProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ImportServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private static Dictionary<ImportCategory, Dictionary<ImportFormat, Type>> ImportServices => new()
    {
        {
            ImportCategory.Classrooms, new Dictionary<ImportFormat, Type>
            {
                { ImportFormat.Excel, typeof(ExcelClassroomImportService) }
            }
        },
        {
            ImportCategory.Students, new Dictionary<ImportFormat, Type>
            {
                { ImportFormat.Excel, typeof(ExcelStudentImportService) }
            }
        },
        {
            ImportCategory.Groups, new Dictionary<ImportFormat, Type>
            {
                { ImportFormat.Excel, typeof(ExcelGroupImportService) }
            }
        },
        {
            ImportCategory.Lecturers, new Dictionary<ImportFormat, Type>
            {
                { ImportFormat.Excel, typeof(ExcelLecturerImportService) }
            }
        },
        {
            ImportCategory.Lessons, new Dictionary<ImportFormat, Type>
            {
                { ImportFormat.Excel, typeof(ExcelLessonImportService) }
            }
        },
        {
            ImportCategory.ClassPeriods, new Dictionary<ImportFormat, Type>
            {
                { ImportFormat.Excel, typeof(ExcelClassPeriodImportService) }
            }
        },
        {
            ImportCategory.ClassVersion, new Dictionary<ImportFormat, Type>
            {
                { ImportFormat.Excel, typeof(ExcelClassVersionImportService) }
            }
        }
    };

    public IImportService Get(ImportCategory category, ImportFormat format)
    {
        var type = ImportServices[category][format];
        return (_serviceProvider.GetRequiredService(type) as IImportService)!;
    }
}