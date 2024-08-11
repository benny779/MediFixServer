using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Users.Entities;
using MediFix.Domain.Users;

namespace MediFix.Application.Users.CreateUser;

public record CreateUserCommand(
    UserType UserType,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    string? PhoneNumber = null) : ICreateCommand<CreateUserResponse>
{
    public ApplicationUser ToApplicationUser()
    {
        return new ApplicationUser
        {
            Type = UserType,
            Email = Email,
            UserName = Email,
            PhoneNumber = PhoneNumber,
            FirstName = FirstName,
            LastName = LastName
        };
    }
}