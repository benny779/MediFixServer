using FluentValidation;
using MediFix.Application.Extensions.Validation.Validators;
using System.Linq.Expressions;

namespace MediFix.Application.Extensions.Validation;

public static class Extensions
{
    public static IRuleBuilderOptions<T, string?> PhoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .SetValidator(new PhoneNumberValidator<T>())
            .WithMessage("'{PropertyName}' is not a valid phone number.");
    }

    public static IRuleBuilderOptions<T, string> ConfirmPassword<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        Expression<Func<T, string>> expression)
    {
        return ruleBuilder
            .NotEmpty()
            .MinimumLength(8)
            .Equal(expression)
            .WithMessage("'{PropertyName}' must be equal to '{ComparisonProperty}'.");
    }

    public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => Guid.TryParse(value, out _))
            .WithMessage("'{PropertyName}' is not a Guid");
    }
}
