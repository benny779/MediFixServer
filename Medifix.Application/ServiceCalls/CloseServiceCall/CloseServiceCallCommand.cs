using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.ServiceCalls.CloseServiceCall;

public record CloseServiceCallCommand(
    Guid ServiceCallId,
    string CloseDetails,
    string QrCode) : ICommand;