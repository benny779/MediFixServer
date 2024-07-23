using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IValidatable
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(next);

        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => Error.Validation(
                GetErrorCode<TRequest>(validationFailure),
                validationFailure.ErrorMessage))
            .ToList();

        if (!errors.Any())
        {
            return await next();
        }

        var validationError = ValidationError.FromErrors(errors);

        return (dynamic)validationError;
    }

    private static string GetErrorCode<T>(ValidationFailure validationFailure)
    {
        if (IsCustomErrorCode(validationFailure))
        {
            return validationFailure.ErrorCode;
        }

        string typeName = typeof(T).Name;
        string propertyName = validationFailure.PropertyName;
        string errorCode = validationFailure.ErrorCode;

        typeName = typeName
            .Replace("Command", string.Empty)
            .Replace("Request", string.Empty);

        errorCode = errorCode.Replace("Validator", string.Empty);

        if (errorCode == propertyName)
        {
            errorCode = string.Empty;
        }

        return $"{typeName}.{propertyName}{(!string.IsNullOrEmpty(errorCode) ? $".{errorCode}" : "")}";
    }

    private static bool IsCustomErrorCode(ValidationFailure validationFailure)
    {
        return !validationFailure.ErrorCode.EndsWith("Command")
               && !validationFailure.ErrorCode.EndsWith("Request");
    }
}
