namespace MediFix.SharedKernel.Results;

public static class ResultExtensions
{
    public static TResponse Match<TResponse>(
        this Result result,
        Func<TResponse> onSuccess,
        Func<Error, TResponse> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    public static TResponse Match<TResponse, TValue>(
        this Result<TValue> result,
        Func<TValue, TResponse> onSuccess,
        Func<Error, TResponse> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }
}
