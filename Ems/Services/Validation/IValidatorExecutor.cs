using Microsoft.AspNetCore.Mvc;

namespace Ems.Services.Validation;

public interface IValidatorExecutor<T, TResult> : IExecutable<IActionResult>
{
}

public interface IValidatorExecutor<out T> : IExecutable<IActionResult>
{
    IValidatorExecutor<T> ContinueWith(Func<T, CancellationToken, Task> callback);
}