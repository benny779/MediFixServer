using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Locations;
using MediFix.Application.SubCategories;
using MediFix.Application.Users;
using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.CreateServiceCall;

internal sealed class CreateServiceCallCommandHandler(
    IClientRepository clientRepository,
    ILocationsRepository locationsRepository,
    ISubCategoryRepository subCategoryRepository,
    IServiceCallRepository serviceCallRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateServiceCallCommand, CreateServiceCallResponse>
{
    public async Task<Result<CreateServiceCallResponse>> Handle(CreateServiceCallCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);

        if (validationResult.Any())
        {
            return ValidationError.FromResults(validationResult);
        }

        var serviceCallResult = ServiceCall.Create(
            ClientId.From(request.ClientId),
            LocationId.From(request.LocationId),
            request.ServiceCallType,
            SubCategoryId.From(request.SubCategoryId),
            request.Details,
            request.Priority);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value!;

        serviceCallRepository.Insert(serviceCall);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateServiceCallResponse(serviceCall.Id);
    }

    private async Task<List<Result>> ValidateRequest(CreateServiceCallCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<Result>
        {
            await clientRepository.GetByIdAsync(ClientId.From(request.ClientId), cancellationToken),
            await locationsRepository.GetByIdAsync(LocationId.From(request.LocationId), cancellationToken),
            await subCategoryRepository.GetByIdAsync(SubCategoryId.From(request.SubCategoryId), cancellationToken)
        };

        if (string.IsNullOrWhiteSpace(request.Details))
        {
            errors.Add(Error.ValueIsNullOrWhiteSpace(request.Details));
        }

        return errors.FindAll(r => r.IsFailure);
    }
}