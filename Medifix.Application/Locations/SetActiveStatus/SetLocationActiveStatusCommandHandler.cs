using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.SetActiveStatus;

internal sealed class SetLocationActiveStatusCommandHandler(
    ILocationsRepository locationsRepository)
    : ICommandHandler<SetLocationActiveStatusCommand>
{
    public async Task<Result> Handle(SetLocationActiveStatusCommand request, CancellationToken cancellationToken)
    {
        var locationResult = await locationsRepository
            .GetByIdAsync(
                request.LocationId,
                cancellationToken);

        if (locationResult.IsFailure)
        {
            return Result.Success();
        }

        var location = locationResult.Value!;

        location.SetActiveStatus(request.IsActive);

        var updateResult = await locationsRepository.UpdateAsync(location, cancellationToken);

        return updateResult.IsSuccess ? Result.Success() : locationResult.Error;
    }
}