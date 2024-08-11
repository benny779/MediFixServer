using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.CreateLocation;

public record CreateLocationResponse(
    Guid Id,
    LocationType LocationType,
    string Name,
    bool IsActive,
    Guid? ParentLocationId) : ICreatedResponse;