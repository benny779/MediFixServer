using MediFix.Domain.Locations;
using MediFix.SharedKernel.Extensions;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.GetLocation;

public record GetLocationResponse(
    Guid Id,
    LocationType LocationType,
    string Name,
    bool IsActive,
    GetLocationResponse? Parent = null)
{
    public GetLocationResponse? Parent { get; private set; } = Parent;

    public static Result<GetLocationResponse> FromDomainLocations(IEnumerable<Location> locations)
    {
        var orderedLocations = locations
            .OrderByDescending(l => (byte)l.LocationType)
            .Select(FromDomainLocation)
            .ToList();

        if (orderedLocations.IsEmpty())
        {
            return Error.EmptyList;
        }

        if (orderedLocations.HasSingle(out var singleLocation))
        {
            return singleLocation;
        }

        return BuildResponseFromLocationList(orderedLocations);
    }

    private static GetLocationResponse BuildResponseFromLocationList(IEnumerable<GetLocationResponse> locations)
    {
        LinkedList<GetLocationResponse> list = new LinkedList<GetLocationResponse>(locations);

        GetLocationResponse locationResponse = list.First!.Value;
        GetLocationResponse? locationPointer = locationResponse;

        for (var pi = list.First.Next; pi is not null; pi = pi.Next)
        {
            locationPointer.Parent = pi.Value;
            locationPointer = locationPointer.Parent;
        }

        return locationResponse;
    }

    public static GetLocationResponse FromDomainLocation(Location location)
    {
        return new GetLocationResponse(
            location.Id.Value,
            location.LocationType,
            location.Name,
            location.IsActive);
    }
}
