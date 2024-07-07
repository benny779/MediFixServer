using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocationChildren;

public record GetLocationChildrenResponse(
    LocationType ChildrenLocationType,
    List<LocationResponse> Values);