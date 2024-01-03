namespace Ems.Infrastructure.Options;

public class QrCodeStudentRecordSessionOptions
{
    public required TimeSpan Expiration { get; set; }
    public required int MaxAttempts { get; set; }
    public required string LogoFileName { get; set; }
}