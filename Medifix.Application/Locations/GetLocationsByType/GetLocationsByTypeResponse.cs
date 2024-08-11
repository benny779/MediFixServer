using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocationsByType;

public record GetLocationsByTypeResponse(
    LocationType LocationType,
    IEnumerable<LocationResponse> Items): IListResponse<LocationResponse>;