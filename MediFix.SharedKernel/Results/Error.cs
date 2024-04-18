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
    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "An 'Unauthorized' error has occurred.")
            => new(code, description, ErrorType.Unauthorized);
    public static Error Forbidden(
        string code = "General.Forbidden",
        string description = "A 'Forbidden' error has occurred.")
           => new(code, description, ErrorType.Forbidden);


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