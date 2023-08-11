namespace Ems.Infrastructure.Options;

public class AccountOptions
{
    public int PasswordLength { get; set; }
    public int AllowedFailedLoginAttempts { get; set; }
    public TimeSpan LinkExpirationTime { get; set; }
    public TimeSpan LockExpirationTime { get; set; }
}