using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocation;

public record GetLocationRequest(
    LocationId LocationId,
    bool IncludeParents) : IQuery<GetLocationResponse>;