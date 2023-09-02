namespace Ems.Services.Validation;

public interface IExecutable<T>
{
    Task<T> Execute(CancellationToken token = default);
}