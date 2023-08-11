namespace Ems.Infrastructure.Options;

public class EmailSenderOptions
{
    public int SmtpPort { get; set; }
    public string SmtpHost { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string From { get; set; }
    public int RetryingCount { get; set; }
    public int DelaySeconds { get; set; }
}