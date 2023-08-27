namespace Ems.Infrastructure.Exceptions;

public class UnauthorizedMethodAccessException : Exception
{
    public UnauthorizedMethodAccessException(string message) : base(message){}
}