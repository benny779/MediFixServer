namespace MediFix.SharedKernel.Results;

public sealed record ValidationError(params Error[] Errors)
    : Error(
        "General.Validation",
        "One or more validation errors occured.",
        ErrorType.Validation)
{
    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new(results.Where(r => r.IsFailure).SelectMany(result =>
        {
            if (result.Error is ValidationError validationError)
            {
                return validationError.Errors;
            }

            return [result.Error];
        }).ToArray());

    public static ValidationError FromErrors(IEnumerable<Error> errors) =>
        new(errors.SelectMany(error =>
        {
            if (error is ValidationError validationError)
            {
                return validationError.Errors;
            }

            return [error];
        }).ToArray());
}
