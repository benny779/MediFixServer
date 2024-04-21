using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocationTypes;

public record GetLocationTypesResponse(
    List<LocationType> LocationTypes);