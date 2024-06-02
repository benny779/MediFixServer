using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.CreateLocation;

internal sealed class CreateLocationCommandHandler(
    ILocationsRepository locationsRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateLocationCommand, CreateLocationResponse>
{
    public async Task<Result<CreateLocationResponse>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var locationId = LocationId.Create();

        Location? parentLocation = await GetParentLocationIfSpecified(request.ParentLocationId, cancellationToken);

        var createLocationResult = Location.Create(
            locationId,
            request.LocationType,
            request.Name,
            parentLocation);

        if (createLocationResult.IsFailure)
        {
            return createLocationResult.Error;
        }

        var location = createLocationResult.Value!;

        locationsRepository.Insert(location);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateLocationResponse(
            location.Id.Value,
            location.LocationType,
            location.Name,
            location.IsActive,
            location.ParentId?.Value);
    }

    private async Task<Location?> GetParentLocationIfSpecified(Guid? locationId, CancellationToken cancellationToken)
    {
        if (locationId is not { } parentLocationId)
        {
            return default;
        }

        var parentLocationResult = await locationsRepository.GetByIdAsync(
            LocationId.From(parentLocationId),
            cancellationToken);

        return parentLocationResult.Value;
    }
}