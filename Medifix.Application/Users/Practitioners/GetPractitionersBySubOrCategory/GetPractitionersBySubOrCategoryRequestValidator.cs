using FluentValidation;

namespace MediFix.Application.Users.Practitioners.GetPractitionersBySubOrCategory;

public sealed class GetPractitionersBySubOrCategoryRequestValidator 
    : AbstractValidator<GetPractitionersBySubOrCategoryRequest>
{
    public GetPractitionersBySubOrCategoryRequestValidator()
    {
        RuleFor(r => r)
            .Must(r => r.CategoryId.HasValue ^ r.SubCategoryId.HasValue)
            .WithErrorCode($"{nameof(GetPractitionersBySubOrCategory)}.QueryParams")
            .WithMessage("You can choose either 'categoryId' or 'subCategoryId'.");
    }
}
