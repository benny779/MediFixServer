namespace MediFix.SharedKernel.Results;

public static class ValidationErrorExtensions
{
    public static ValidationError ToValidationError(this IEnumerable<Error> errors)
    {
        return ValidationError.FromErrors(errors);
    }
}