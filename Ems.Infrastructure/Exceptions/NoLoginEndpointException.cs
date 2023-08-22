namespace Ems.Infrastructure.Exceptions;

public class NoLoginEndpointException : Exception
{
    public NoLoginEndpointException(string message) : base(message) { }
}