using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.UpdateLocation;

internal sealed class UpdateLocationCommandHandler(
    ILocationsRepository locationsRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateLocationCommand>
{
    public async Task<Result> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var locationId = LocationId.From(request.LocationId);

        var locationResult = await locationsRepository.GetByIdAsync(locationId, cancellationToken);

        if (locationResult.IsFailure)
        {
            return locationResult;
        }

        var location = locationResult.Value;

        if (await locationsRepository
                .SameTypeAndNameAlreadyExist(location, cancellationToken))
        {
            return SameTypeAndNameAlreadyExist(location.LocationType, location.Name);
        }

        var changeNameResult = location.Update(request.Name, request.IsActive);

        if (changeNameResult.IsFailure)
        {
            return changeNameResult.Error;
        }

        locationsRepository.Update(location);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static Error SameTypeAndNameAlreadyExist(LocationType locationType, string name) =>
        Error.Conflict(
            "Location.SameTypeAndNameAlreadyExist",
            $"A location of type '{locationType}' and the name '{name}' already exists.");
}