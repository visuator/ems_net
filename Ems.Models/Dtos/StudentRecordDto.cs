using Ems.Core.Entities.Enums;

namespace Ems.Models.Dtos;

public class StudentRecordDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }

    public StudentDto Student { get; set; }

    public Guid ClassId { get; set; }

    public ClassDto Class { get; set; }

    public StudentRecordStatus Status { get; set; }
    public StudentRecordType Type { get; set; }
    public Guid? StudentRecordSessionId { get; set; }
    public StudentRecordSessionDto? StudentRecordSession { get; set; }
}