using FluentValidation;
using FluentValidation.Validators;

namespace MediFix.Application.Extensions.Validation.Validators;

public class PhoneNumberValidator<T> : PropertyValidator<T, string?>
{
    public override string Name => "PhoneNumberValidator";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is { Length: < 8 or > 10 })
        {
            return false;
        }

        if (value.Any(c => !char.IsDigit(c)))
        {
            return false;
        }

        return true;
    }
}
