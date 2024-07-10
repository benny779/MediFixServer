using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.ChangeLocationName;

internal sealed class ChangeLocationNameCommandHandler(
    ILocationsRepository locationsRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ChangeLocationNameCommand>
{
    public async Task<Result> Handle(ChangeLocationNameCommand request, CancellationToken cancellationToken)
    {
        var locationResult = await locationsRepository.GetByIdAsync(request.LocationId, cancellationToken);

        if (locationResult.IsFailure)
        {
            return locationResult;
        }

        var location = locationResult.Value;

        if (NamesAreEqual(request, location))
        {
            return Result.Success();
        }

        if (await locationsRepository
                .SameTypeAndNameAlreadyExist(location, cancellationToken))
        {
            return SameTypeAndNameAlreadyExist(location.LocationType, location.Name);
        }

        var changeNameResult = location.ChangeName(request.Name);

        if (changeNameResult.IsFailure)
        {
            return changeNameResult.Error;
        }

        locationsRepository.Update(location);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static bool NamesAreEqual(ChangeLocationNameCommand request, Location location)
    {
        return location.Name.Equals(request.Name);
    }

    private static Error SameTypeAndNameAlreadyExist(LocationType locationType, string name) =>
        Error.Conflict(
            "Location.SameTypeAndNameAlreadyExist",
            $"A location of type '{locationType}' and the name '{name}' already exists.");
}