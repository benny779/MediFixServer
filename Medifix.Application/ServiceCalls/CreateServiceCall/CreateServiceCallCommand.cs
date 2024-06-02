using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.ServiceCalls;

namespace MediFix.Application.ServiceCalls.CreateServiceCall;

public record CreateServiceCallCommand(
    Guid ClientId,
    Guid LocationId,
    ServiceCallType ServiceCallType,
    Guid SubCategoryId,
    string Details,
    ServiceCallPriority Priority = ServiceCallPriority.Low) : ICommand<CreateServiceCallResponse>;