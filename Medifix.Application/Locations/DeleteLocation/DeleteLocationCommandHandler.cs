using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.DeleteLocation;

internal sealed class DeleteLocationCommandHandler(
    ILocationsRepository locationsRepository)
    : ICommandHandler<DeleteLocationCommand>
{
    public async Task<Result> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        return await locationsRepository.DeleteByIdAsync(request.LocationId, cancellationToken);
    }
}