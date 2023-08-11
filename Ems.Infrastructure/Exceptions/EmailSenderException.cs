namespace Ems.Infrastructure.Exceptions;

public class EmailSenderException : Exception
{
    public EmailSenderException(string message) : base(message)
    {
    }
}