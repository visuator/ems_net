using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ems.Services;

public class ValidatorResolverService<TModel>
{
    private readonly IValidator<TModel> _validator;
    private TModel _model;

    public ValidatorResolverService(IValidator<TModel> validator)
    {
        _validator = validator;
    }

    public ValidatorResolverService<TModel> ForModel(TModel model)
    {
        _model = model;
        return this;
    }

    public ValidationStrategyBuilder<TModel> HasModelStateFallback(ModelStateDictionary modelState)
    {
        return new(_validator, _model, modelState);
    }

    public class ValidationStrategyBuilder<TModel>
    {
        private readonly ModelStateDictionary _modelState;
        private readonly IValidator<TModel> _validator;
        private readonly TModel _model;
        private readonly List<ValidationStrategy<TModel>> _strategies = new();

        public ValidationStrategyBuilder(IValidator<TModel> validator, TModel model, ModelStateDictionary modelState)
        {
            _validator = validator;
            _model = model;
            _modelState = modelState;
        }

        public ValidationStrategyBuilder<TModel> OnSuccess(Func<CancellationToken, TModel, Task> action)
        {
            _strategies.Add(new NoResultValidationStrategy<TModel>(_model, action));
            return this;
        }

        public ValidationStrategyBuilder<TModel> OnSuccess<TResult>(
            Func<CancellationToken, TModel, Task<TResult>> action)
        {
            _strategies.Add(new ResultValidationStrategy<TModel, TResult>(_model, action));
            return this;
        }

        public async Task<IActionResult> Execute(CancellationToken token = new())
        {
            var result = await _validator.ValidateAsync(_model, token);
            if (!result.IsValid)
                return new BadRequestObjectResult(new ValidationProblemDetails(_modelState));
            foreach (var str in _strategies.OfType<NoResultValidationStrategy<TModel>>().ToList())
                await str.Execute(token);
            return new OkResult();
        }

        public async Task<IActionResult> Execute<TResult>(CancellationToken token = new())
        {
            var result = await _validator.ValidateAsync(_model, token);
            if (!result.IsValid)
                return new BadRequestObjectResult(new ValidationProblemDetails(_modelState));
            var firstStrategy = _strategies.OfType<ResultValidationStrategy<TModel, TResult>>().First();
            return new OkObjectResult(await firstStrategy.Execute(token));
        }

        public abstract class ValidationStrategy<TModel>
        {
            protected readonly TModel Model;

            protected ValidationStrategy(TModel model)
            {
                Model = model;
            }

            public abstract Task Execute(CancellationToken token = new());
        }

        private class NoResultValidationStrategy<TModel> : ValidationStrategy<TModel>
        {
            private readonly Func<CancellationToken, TModel, Task> _onSuccess;

            public NoResultValidationStrategy(TModel model,
                Func<CancellationToken, TModel, Task> onSuccess) : base(model)
            {
                _onSuccess = onSuccess;
            }

            public override async Task Execute(CancellationToken token = new())
            {
                await _onSuccess(token, Model);
            }
        }

        private class ResultValidationStrategy<TModel, TResult> : ValidationStrategy<TModel>
        {
            private readonly Func<CancellationToken, TModel, Task<TResult>> _onSuccess;

            public ResultValidationStrategy(TModel model,
                Func<CancellationToken, TModel, Task<TResult>> onSuccess) : base(model)
            {
                _onSuccess = onSuccess;
            }

            public override async Task<TResult> Execute(CancellationToken token = new())
            {
                return await _onSuccess(token, Model);
            }
        }
    }
}