namespace MediFix.SharedKernel.Results;

public sealed record ValidationError(params Error[] Errors)
    : Error(
        "General.Validation",
        "One or more validation errors occured.",
        ErrorType.Validation)
{
    public static ValidationError FromResults(IEnumerable<Result> results) =>
        results.Where(r => r.IsFailure)
            .Select(r => r.Error)
            .ToValidationError();

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
