using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Locations;

public record LocationsWithTypeResponse(IEnumerable<LocationWithTypeResponse> Items)
    : IListResponse<LocationWithTypeResponse>;