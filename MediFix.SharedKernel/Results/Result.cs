namespace MediFix.SharedKernel.Results;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid Error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result FromTryCatch(Action action)
    {
        try
        {
            action();
            return Success();
        }
        catch (Exception ex)
        {
            return Failure(Error.Failure(code: "General.Exception", description: ex.Message));
        }
    }

    public static Result<TResult> FromTryCatch<TResult>(Func<TResult> action)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            return Failure<TResult>(Error.Failure(code: "General.Exception", description: ex.Message));
        }
    }

    public static async Task<Result> FromTryCatchAsync(Func<Task> action)
    {
        try
        {
            await action();
            return Success();
        }
        catch (Exception ex)
        {
            return Failure(Error.Failure(code: "General.Exception", description: ex.Message));
        }
    }

    public static async Task<Result<TResult>> FromTryCatchAsync<TResult>(Func<Task<TResult>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return Failure<TResult>(Error.Failure(code: "General.Exception", description: ex.Message));
        }
    }

    public static Result Success() => new(true, Error.None);
    public static Result<TResult> Success<TResult>(TResult value) => new(true, Error.None, value);
    public static Result Failure(Error error) => new(false, error);

#pragma warning disable CS8604 // Possible null reference argument.
    public static Result<TResult> Failure<TResult>(Error error) => new(false, error, default);
#pragma warning restore CS8604 // Possible null reference argument.

    public static implicit operator bool(Result result) => result.IsSuccess;
}

public class Result<TValue> : Result
{
    public TValue Value { get; }

    internal Result(bool isSuccess, Error error, TValue value)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.Unexpected("Value.Null"));

    public static implicit operator TValue?(Result<TValue> result) => result.Value;

    public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
}