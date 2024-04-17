using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.GetLocationChildren;

public record GetLocationChildrenRequest(LocationId LocationId)
    : IQuery<GetLocationChildrenResponse>;