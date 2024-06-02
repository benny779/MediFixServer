using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.ServiceCalls.CreateServiceCall;

public record CreateServiceCallResponse(Guid Id) : ICreatedResponse;