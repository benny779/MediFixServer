using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Password) : ICommand<CreateUserResponse>;