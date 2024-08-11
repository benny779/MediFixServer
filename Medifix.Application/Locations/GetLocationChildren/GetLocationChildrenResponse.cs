using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocationChildren;

public record GetLocationChildrenResponse(
    LocationType ChildrenLocationType,
    IEnumerable<LocationResponse> Items) : IListResponse<LocationResponse>;