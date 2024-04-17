using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.GetLocation;

internal sealed class GetLocationRequestHandler(
    ILocationsRepository locationsRepository)
    : IQueryHandler<GetLocationRequest, GetLocationResponse>
{
    public async Task<Result<GetLocationResponse>> Handle(GetLocationRequest request, CancellationToken cancellationToken)
    {
        if (request.IncludeParents)
        {
            var locations = await locationsRepository.GetByIdWithParentsAsync(
                        request.LocationId,
                        cancellationToken);

            return locations.IsFailure
                ? locations.Error
                : GetLocationResponse.FromDomainLocations(locations.Value!);
        }

        var location = await locationsRepository.GetByIdAsync(
            request.LocationId,
            cancellationToken);

        return location.IsFailure
            ? location.Error
            : GetLocationResponse.FromDomainLocation(location.Value!);
    }
}