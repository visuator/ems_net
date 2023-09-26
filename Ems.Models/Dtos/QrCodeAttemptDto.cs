using Ems.Core.Entities.Enums;
using Ems.Infrastructure.Attributes;

namespace Ems.Models.Dtos;

public class QrCodeAttemptDto
{
    [NotMapWhenInRole(Role.Student)]
    [NotMapWhenInRole(Role.Lecturer)]
    public string Content { get; set; }
    public QrCodeAttemptStatus Status { get; set; }
    public Guid QrCodeStudentRecordSessionId { get; set; }
    public QrCodeStudentRecordSessionDto QrCodeStudentRecordSession { get; set; } 
    public Guid Id { get; set; }
}