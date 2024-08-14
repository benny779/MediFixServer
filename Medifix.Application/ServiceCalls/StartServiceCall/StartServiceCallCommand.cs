using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.ServiceCalls.StartServiceCall;

public record StartServiceCallCommand(Guid ServiceCallId) : ICommand;