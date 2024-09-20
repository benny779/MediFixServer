using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocationsByType;

public record GetLocationsByTypeRequest(LocationType LocationType, bool WithInactive)
    : IQuery<GetLocationsByTypeResponse>;