using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MediFix.SharedKernel.Results;

public record Error
{
    public ErrorType Type { get; set; }
    public string Code { get; set; }
    public string? Description { get; set; }


    public static readonly Error None = new(string.Empty, default, ErrorType.Failure);
    public static readonly Error NullValue = Validation("Error.NullValue", "The specified result value is null.");
    public static readonly Error EmptyList = Validation("Error.EmptyList", "The specified list is empty.");

    public static Error Failure(
        string code = "General.Failure",
        string description = "A failure has occurred.")
            => new(code, description, ErrorType.Failure);
    public static Error Unexpected(
        string code = "General.Unexpected",
        string description = "An unexpected error has occurred.")
            => new(code, description, ErrorType.Unexpected);
    public static Error Validation(
        string code = "General.Validation",
        string description = "A validation error has occurred.")
            => new ValidationError(new Error(code, description, ErrorType.Validation));
    public static Error Conflict(
        string code = "General.Conflict",
        string description = "A conflict error has occurred.")
            => new(code, description, ErrorType.Conflict);

    public static Error AlreadyExists<TEntity>(string propertyName)
        => new(
            $"{typeof(TEntity).Name}.AlreadyExists",
            $"An entity {typeof(TEntity).Name} with the given {propertyName} already exists.",
            ErrorType.NotFound);

    public static Error NotFound(
        string code = "General.NotFound",
        string description = "A 'Not Found' error has occurred.")
            => new(code, description, ErrorType.NotFound);

    public static Error EntityNotFound<TEntity>()
        => new(
            $"{typeof(TEntity).Name}.NotFound",
            $"{typeof(TEntity).Name} entity not found.",
            ErrorType.NotFound);

    public static Error EntityNotFound<TEntity>(object? id)
        => new(
            $"{typeof(TEntity).Name}.WithId.NotFound",
            $"The entity {typeof(TEntity).Name} with id '{id}' was not found.",
            ErrorType.NotFound);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
    public static Error ValueIsNull(
        [NotNull] object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => Validation(
            $"Argument.Null.{paramName}",
            $"Value cannot be null: {paramName}.");

    public static Error ValueIsNullOrEmpty(
            [NotNull] object? argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => Validation(
            $"Argument.NullOrEmpty.{paramName}",
            $"Value cannot be null or empty: {paramName}.");

    public static Error ValueIsNullOrWhiteSpace(
        [NotNull] object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        => Validation(
            $"Argument.NullOrWhiteSpace.{paramName}",
            $"Value cannot be null or white space: {paramName}.");
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "An 'Unauthorized' error has occurred.")
            => new(code, description, ErrorType.Unauthorized);
    public static Error Forbidden(
        string code = "General.Forbidden",
        string description = "A 'Forbidden' error has occurred.")
           => new(code, description, ErrorType.Forbidden);

    public static Error FromException(Exception exception)
    {
        var errorDescription = exception.InnerException is not null
            ? $"{exception.Message} | Inner Exception: {exception.InnerException.Message}"
            : exception.Message;

        return Failure(code: exception.GetType().Name, description: errorDescription);
    }


    protected Error(string code, string? description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public static implicit operator Result(Error error) => Result.Failure(error);

    public static implicit operator string(Error error) => error.Code;
}

public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
}