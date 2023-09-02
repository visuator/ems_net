namespace Ems.Services.Validation;

public interface IValidatorExecutorBuilder<T>
{
    IValidatorExecutor<T> WithAsyncCallback(Func<T, CancellationToken, Task> callback);

    IValidatorExecutor<T, TResult>
        WithAsyncCallback<TResult>(Func<T, CancellationToken, Task<TResult>> callback);
}