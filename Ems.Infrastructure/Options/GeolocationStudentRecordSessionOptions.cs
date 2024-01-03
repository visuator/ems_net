namespace Ems.Infrastructure.Options;

public class GeolocationStudentRecordSessionOptions
{
    public required int Threshold { get; set; }
    public required TimeSpan Expiration { get; set; }
}