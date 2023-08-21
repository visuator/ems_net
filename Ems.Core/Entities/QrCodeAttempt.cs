using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Constants;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;

namespace Ems.Core.Entities;

[Table("qr_code_attempts", Schema = Schemas.Main)]
public class QrCodeAttempt : EntityBase, ISingleKeyEntity
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("image")]
    public byte[] Image { get; set; }
    [Column("content")]
    public string Content { get; set; }
    [Column("status")]
    public QrCodeAttemptStatus Status { get; set; }
    [Column("qr_code_student_record_session_id")]
    public Guid QrCodeStudentRecordSessionId { get; set; }
    public QrCodeStudentRecordSession QrCodeStudentRecordSession { get; set; }
}