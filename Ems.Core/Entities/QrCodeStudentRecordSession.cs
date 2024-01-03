using Ems.Core.Entities.Abstractions;

namespace Ems.Core.Entities;

public class QrCodeStudentRecordSession : StudentRecordSession
{
    public ICollection<QrCodeAttempt> Attempts { get; set; } = default!;
}