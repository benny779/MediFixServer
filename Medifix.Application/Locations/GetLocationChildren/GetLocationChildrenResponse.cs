using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocationChildren;

public record GetLocationChildrenResponse(
    LocationType ChildrenLocationType,
    List<LocationChildren> Values);

public record LocationChildren(
    Guid Id,
    string Name,
    bool IsActive)
{
    public static LocationChildren FromDomainLocation(Location location)
    {
        return new LocationChildren(
            location.Id.Value,
            location.Name,
            location.IsActive);
    }
}