namespace MediFix.SharedKernel.Results;

public sealed record ValidationError(params Error[] Errors)
    : Error(
        "General.Validation",
        "One or more validation errors occured.",
        ErrorType.Validation)
{
    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new(results.Where(r => r.IsFailure).SelectMany(r =>
        {
            if (r.Error is ValidationError v)
            {
                return v.Errors;
            }

            return [r.Error];
        }).ToArray());
}
