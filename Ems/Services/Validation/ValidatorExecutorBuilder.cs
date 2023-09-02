using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ems.Services.Validation;

public class ValidatorExecutorBuilder<T> : IValidatorExecutorBuilder<T>
{
    private readonly T _model;
    private readonly IValidator<T> _validator;
    private readonly ModelStateDictionary? _modelState;

    public ValidatorExecutorBuilder(T model, IValidator<T> validator, ModelStateDictionary? modelState = null)
    {
        _model = model;
        _validator = validator;
        _modelState = modelState;
    }

    public IValidatorExecutor<T> WithAsyncCallback(Func<T, CancellationToken, Task> callback)
    {
        return new ActionValidatorExecutor<T>(_model, _validator, callback, _modelState);
    }

    public IValidatorExecutor<T, TResult> WithAsyncCallback<TResult>(Func<T, CancellationToken, Task<TResult>> callback)
    {
        return new ResultValidatorExecutor<T, TResult>(_model, _validator, callback, _modelState);
    }
}