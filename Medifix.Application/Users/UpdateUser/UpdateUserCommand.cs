using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? PhoneNumber) : ICommand;