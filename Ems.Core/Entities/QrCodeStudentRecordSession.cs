namespace Ems.Core.Entities;

public class QrCodeStudentRecordSession : StudentRecordSession
{
    public List<QrCodeAttempt> Attempts { get; set; }
}