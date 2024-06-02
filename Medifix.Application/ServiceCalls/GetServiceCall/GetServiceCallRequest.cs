using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.ServiceCalls.GetServiceCall;

public record GetServiceCallRequest(Guid Id) : IQuery<GetServiceCallResponse>;