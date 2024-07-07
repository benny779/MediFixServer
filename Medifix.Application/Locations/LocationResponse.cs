using MediFix.Domain.Locations;

namespace MediFix.Application.Locations;

public record LocationResponse(
    Guid Id,
    string Name,
    bool IsActive)
{
    public static LocationResponse FromDomainLocation(Location location)
    {
        return new LocationResponse(
            location.Id.Value,
            location.Name,
            location.IsActive);
    }
}