using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocationTypes;

public record GetLocationTypesResponse(IEnumerable<LocationType> Items) : IListResponse<LocationType>;