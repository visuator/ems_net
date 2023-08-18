namespace Ems.Infrastructure.Options;

public class GeolocationStudentRecordSessionOptions
{
    public int Threshold { get; set; }
    public TimeSpan Expiration { get; set; }
}