using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Utils.Email.TestEmail;

public record TestEmailCommand(
    string To,
    string Subject,
    string Body) : ICommand;