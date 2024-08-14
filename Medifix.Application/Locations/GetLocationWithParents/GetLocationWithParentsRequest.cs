using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Locations.GetLocationWithParents;

public record GetLocationWithParentsRequest(Guid Id) : IQuery<LocationsWithTypeResponse>;