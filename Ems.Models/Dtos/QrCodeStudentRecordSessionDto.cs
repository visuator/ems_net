namespace Ems.Models.Dtos;

public class QrCodeStudentRecordSessionDto : StudentRecordSessionDto
{
    public List<QrCodeAttemptDto> Attempts { get; set; }
}