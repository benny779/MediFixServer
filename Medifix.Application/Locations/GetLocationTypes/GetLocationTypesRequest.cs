using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Locations.GetLocationTypes;

public record GetLocationTypesRequest : IQuery<GetLocationTypesResponse>;