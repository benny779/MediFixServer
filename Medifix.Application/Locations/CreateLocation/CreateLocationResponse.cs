using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.CreateLocation;

public record CreateLocationResponse(
    Guid Id,
    LocationType LocationType,
    string Name,
    Guid? ParentLocationId);