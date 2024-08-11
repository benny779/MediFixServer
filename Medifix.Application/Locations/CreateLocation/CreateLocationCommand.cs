using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.CreateLocation;

public record CreateLocationCommand(
    LocationType LocationType,
    string Name,
    Guid? ParentLocationId) : ICreateCommand<CreateLocationResponse>;