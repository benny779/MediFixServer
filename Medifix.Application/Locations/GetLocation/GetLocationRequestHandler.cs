using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.GetLocation;

internal sealed class GetLocationRequestHandler(
    ILocationsRepository locationsRepository)
    : IQueryHandler<GetLocationRequest, LocationWithTypeResponse>
{
    public async Task<Result<LocationWithTypeResponse>> Handle(GetLocationRequest request, CancellationToken cancellationToken)
    {
        var location = await locationsRepository.GetByIdAsync(
            request.LocationId,
            cancellationToken);

        return location.IsFailure
            ? location.Error
            : LocationWithTypeResponse.FromDomainLocation(location.Value);
    }
}