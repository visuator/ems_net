namespace Ems.Infrastructure.Exceptions;

public class StartupException : Exception
{
    public StartupException(string message) : base(message)
    {
    }
}