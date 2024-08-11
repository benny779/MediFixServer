using FluentValidation;
using MediFix.Application.Locations;
using MediFix.Application.SubCategories;
using MediFix.Application.Users;
using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.Users;

namespace MediFix.Application.ServiceCalls.CreateServiceCall;

public class CreateServiceCallCommandValidator : AbstractValidator<CreateServiceCallCommand>
{
    public CreateServiceCallCommandValidator(
        IClientRepository clientRepository,
        ILocationsRepository locationsRepository,
        ISubCategoryRepository subCategoryRepository)
    {
        RuleFor(x => x.ClientId)
            .MustAsync(async (clientId, cancellation) =>
            {
                var result = await clientRepository.GetByIdAsync(ClientId.From(clientId), cancellation);
                return result.IsSuccess;
            }).WithMessage("Invalid '{PropertyName}'");

        RuleFor(x => x.LocationId)
            .MustAsync(async (locationId, cancellation) =>
            {
                var result = await locationsRepository.GetByIdAsync(LocationId.From(locationId), cancellation);
                return result.IsSuccess;
            }).WithMessage("Invalid '{PropertyName}'");

        RuleFor(x => x.SubCategoryId)
            .MustAsync(async (subCategoryId, cancellation) =>
            {
                var result = await subCategoryRepository.GetByIdAsync(SubCategoryId.From(subCategoryId), cancellation);
                return result.IsSuccess;
            }).WithMessage("Invalid '{PropertyName}'");

        RuleFor(x => x.Details)
            .NotEmpty();
    }
}
