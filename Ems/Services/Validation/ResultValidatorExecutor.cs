using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ems.Services.Validation;

public class ResultValidatorExecutor<T, TResult> : IValidatorExecutor<T, TResult>
{
    private readonly T _model;
    private readonly IValidator<T> _validator;
    private readonly ModelStateDictionary? _modelState;
    private readonly List<Func<T, CancellationToken, Task<TResult>>> _callbacks = new();
    
    public ResultValidatorExecutor(T model, IValidator<T> validator, Func<T, CancellationToken, Task<TResult>> callback, ModelStateDictionary? modelState = null)
    {
        _model = model;
        _validator = validator;
        _modelState = modelState;
        _callbacks.Add(callback);
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