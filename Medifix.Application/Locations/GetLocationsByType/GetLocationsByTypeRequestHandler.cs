using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.GetLocationsByType;

internal sealed class GetLocationsByTypeRequestHandler(
    ILocationsRepository locationsRepository)
    : IQueryHandler<GetLocationsByTypeRequest, GetLocationsByTypeResponse>
{
    public async Task<Result<GetLocationsByTypeResponse>> Handle(GetLocationsByTypeRequest request, CancellationToken cancellationToken)
    {
        var locationsResult = await locationsRepository
            .GetByType(request.LocationType, cancellationToken);

        if (locationsResult.IsFailure)
        {
            return locationsResult.Error;
        }

        return new GetLocationsByTypeResponse(
            request.LocationType,
            locationsResult.Value!
                .Select(loc => new LocationByTypeResponse(
                    loc.Id.Value,
                    loc.Name,
                    loc.IsActive))
                .ToList());
    }
}