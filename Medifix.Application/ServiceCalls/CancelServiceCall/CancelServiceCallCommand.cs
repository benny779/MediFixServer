using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.ServiceCalls.CancelServiceCall;

public record CancelServiceCallCommand(Guid Id) : ICommand;