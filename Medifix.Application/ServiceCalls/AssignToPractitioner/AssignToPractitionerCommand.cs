using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;

namespace MediFix.Application.ServiceCalls.AssignToPractitioner;

public record AssignToPractitionerCommand(
    ServiceCallId ServiceCallId,
    PractitionerId PractitionerId) : ICommand;