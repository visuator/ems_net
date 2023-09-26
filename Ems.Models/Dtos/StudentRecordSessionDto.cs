using Ems.Core.Entities.Enums;

namespace Ems.Models.Dtos;

public class StudentRecordSessionDto
{
    public Guid Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndingAt { get; set; }
    public Guid ClassId { get; set; }
    public ClassDto Class { get; set; }
    public StudentRecordSessionType Type { get; set; }
    public Guid LecturerId { get; set; }
    public LecturerDto Lecturer { get; set; }
}