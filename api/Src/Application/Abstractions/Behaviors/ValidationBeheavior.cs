using DDD.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace DDD.Application.Abstractions.Behaviors;

/// <summary>
/// add model validation to the request Pipeline for existing <see cref="AbstractValidator"/>>
/// to trigger on each request start
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> Validation)
    {
        _validators = Validation;
    }
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationFailures = await Task.WhenAll(_validators.Select(a => a.ValidateAsync(context)));
        var error = validationFailures
            .Where(a => !a.IsValid)
            .SelectMany(a => a.Errors)
            .Select(a => new ValidationError(a.PropertyName, a.ErrorMessage))
            .ToList();

        if (error.Count != 0)
            throw new BaseValidationException(error);

        return await next();
    }
}
