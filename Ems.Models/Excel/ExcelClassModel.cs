using Ems.Core.Entities.Enums;

namespace Ems.Models.Excel;

public class ExcelClassModel
{
    public Quarter Quarter { get; set; }
    public DayOfWeek Day { get; set; }
    public string GroupName { get; set; }
    public string ClassPeriodName { get; set; }
    public string LecturerFullName { get; set; }
    public string LessonName { get; set; }
    public string ClassroomName { get; set; }
    public ClassType Type { get; set; }
}