using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Identity;

namespace MediFix.Application.Extensions;

public static class IdentityErrorExtensions
{
    public static ValidationError ToValidationError(this IEnumerable<IdentityError> identityErrors)
    {
        var errors = identityErrors
            .Select(e => Error.Validation(e.Code, e.Description) as ValidationError)
            .SelectMany(e => e!.Errors)
            .ToArray();

        return new ValidationError(errors);
    }
}
