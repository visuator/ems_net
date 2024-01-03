using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class QrCodeAttempt : EntityBase, ISingleKeyEntity
{
    public byte[] Image { get; set; } = default!;
    public string Content { get; set; } = default!;
    public QrCodeAttemptStatus Status { get; set; }
    public Guid QrCodeStudentRecordSessionId { get; set; }
    public QrCodeStudentRecordSession QrCodeStudentRecordSession { get; set; } = default!;
    public Guid Id { get; set; }
}