using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.GetLocationChildren;

internal sealed class GetLocationChildrenHandler(
    ILocationsRepository locationsRepository)
    : IQueryHandler<GetLocationChildrenRequest, GetLocationChildrenResponse>
{
    public async Task<Result<GetLocationChildrenResponse>> Handle(
        GetLocationChildrenRequest request,
        CancellationToken cancellationToken)
    {
        var locationsResult = await locationsRepository.GetChildren(request.LocationId, cancellationToken);

        if (locationsResult.IsFailure)
        {
            return locationsResult.Error;
        }

        var locations = locationsResult.Value!;
        
        var locationType = locations
            .Select(loc => loc.LocationType)
            .FirstOrDefault();
        
        var list = locations
            .Select(LocationChildren.FromDomainLocation)
            .ToList();

        return new GetLocationChildrenResponse(locationType, list);
    }
}