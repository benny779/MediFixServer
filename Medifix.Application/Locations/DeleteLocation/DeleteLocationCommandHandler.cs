using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.DeleteLocation;

internal sealed class DeleteLocationCommandHandler(
    ILocationsRepository locationsRepository)
    : ICommandHandler<DeleteLocationCommand>
{
    public async Task<Result> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        // TODO: Ensure the location can be deleted
        if (!LocationCanBeDeleted(request.LocationId))
        {
            return Error.Validation(
                "Location.DeleteNotAllowed",
                "The location cannot be deleted.");
        }

        return await locationsRepository.DeleteByIdAsync(request.LocationId, cancellationToken);
    }

    private bool LocationCanBeDeleted(LocationId locationId)
    {
        throw new NotImplementedException();
    }
}