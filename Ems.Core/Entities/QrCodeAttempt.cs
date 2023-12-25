using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class QrCodeAttempt : EntityBase, ISingleKeyEntity
{
    public byte[] Image { get; set; }
    public string Content { get; set; }
    public QrCodeAttemptStatus Status { get; set; }
    public Guid QrCodeStudentRecordSessionId { get; set; }
    public QrCodeStudentRecordSession QrCodeStudentRecordSession { get; set; }
    public Guid Id { get; set; }
}