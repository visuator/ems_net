using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class QrCodeStudentRecordSession : StudentRecordSession
{
    public required List<QrCodeAttempt> Attempts { get; set; }
}