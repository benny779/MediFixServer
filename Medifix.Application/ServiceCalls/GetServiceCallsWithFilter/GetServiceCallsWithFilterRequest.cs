using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.ServiceCalls.GetServiceCallsWithFilter;

public record GetServiceCallsWithFilterRequest(
    Guid? ClientId,
    Guid? PractitionerId)
    : IQuery<ServiceCallsResponse>;