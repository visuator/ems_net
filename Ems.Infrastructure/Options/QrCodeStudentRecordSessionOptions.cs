namespace Ems.Infrastructure.Options;

public class QrCodeStudentRecordSessionOptions
{
    public TimeSpan Expiration { get; set; }
    public TimeSpan SlidingTime { set; get; }
    public int MaxAttempts { get; set; }
    public string LogoFileName { get; set; }
}