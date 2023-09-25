using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ems.Services.Validation;

public class ActionValidatorExecutor<T> : IValidatorExecutor<T>
{
    private readonly List<Func<T, CancellationToken, Task>> _callbacks = new();
    private readonly T _model;
    private readonly ModelStateDictionary? _modelState;
    private readonly IValidator<T> _validator;

    public ActionValidatorExecutor(T model, IValidator<T> validator, Func<T, CancellationToken, Task> callback,
        ModelStateDictionary? modelState = null)
    {
        _model = model;
        _validator = validator;
        _modelState = modelState;
        _callbacks.Add(callback);
    }

    public IValidatorExecutor<T> ContinueWith(Func<T, CancellationToken, Task> callback)
    {
        _callbacks.Add(callback);
        return this;
    }

    public async Task<IActionResult> Execute(CancellationToken token = default)
    {
        var result = await _validator.ValidateAsync(_model, token);
        if (result.IsValid)
        {
            foreach (var callback in _callbacks)
                await callback(_model, token);
            return new OkResult();
        }

        foreach (var error in result.Errors)
            _modelState?.AddModelError(error.PropertyName, error.ErrorMessage);
        if (_modelState is not null)
            return new BadRequestObjectResult(new ValidationProblemDetails(_modelState));
        return new BadRequestResult();
    }
}