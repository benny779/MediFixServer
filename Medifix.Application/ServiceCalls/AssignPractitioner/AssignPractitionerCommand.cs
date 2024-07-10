using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.ServiceCalls.AssignPractitioner;

public record AssignPractitionerCommand(
    Guid ServiceCallId,
    Guid PractitionerId) : ICommand;