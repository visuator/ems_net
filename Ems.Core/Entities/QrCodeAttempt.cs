using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

public class QrCodeAttempt : EntityBase, ISingleKeyEntity
{
    public required byte[] Image { get; set; }
    public required string Content { get; set; }
    public required QrCodeAttemptStatus Status { get; set; }
    public required Guid QrCodeStudentRecordSessionId { get; set; }
    public required QrCodeStudentRecordSession QrCodeStudentRecordSession { get; set; }
    public required Guid Id { get; set; }
}