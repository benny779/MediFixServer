using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.ServiceCalls.CloseServiceCallCommand;

public record CloseServiceCallCommand(
    Guid ServiceCallId,
    string CloseDetails,
    string QrCode) : ICommand;