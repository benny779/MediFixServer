using MediFix.Domain.Locations;

namespace MediFix.Application.Locations;

public record LocationWithTypeResponse(
    Guid Id,
    LocationType LocationType,
    string Name,
    bool IsActive)
{
    public static LocationWithTypeResponse FromDomainLocation(Location location)
    {
        return new LocationWithTypeResponse(
            location.Id.Value,
            location.LocationType,
            location.Name,
            location.IsActive);
    }
}