using FluentValidation;

namespace MediFix.Application.Expertises.GetExpertiseBy;

public sealed class GetExpertiseByRequestValidator : AbstractValidator<GetExpertiseByRequest>
{
    public GetExpertiseByRequestValidator()
    {
        RuleFor(r => r)
            .Must(r => r.CategoryId.HasValue ^ r.PractitionerId.HasValue)
            .WithErrorCode($"{nameof(GetExpertiseByRequest)}.QueryParams")
            .WithMessage("You can choose either 'categoryId' or 'practitionerId'.");
    }
}