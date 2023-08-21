namespace Ems.Models;

public class QrCodeStudentRecordSessionOptionsModel
{
    public TimeSpan SlidingTime { set; get; }
    public int MaxAttempts { get; set; }
}