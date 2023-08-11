namespace Ems.Infrastructure.Exceptions;

public class JobException : Exception
{
    public JobException(string message) : base(message)
    {
    }
}