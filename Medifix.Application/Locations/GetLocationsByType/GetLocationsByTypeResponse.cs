using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocationsByType;

public record GetLocationsByTypeResponse(
    LocationType LocationType,
    List<LocationByTypeResponse> Locations);

public record LocationByTypeResponse(
    Guid Id,
    string Name,
    bool IsActive);