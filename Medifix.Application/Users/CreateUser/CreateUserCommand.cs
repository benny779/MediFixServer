using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Users.Entities;
using MediFix.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace MediFix.Application.Users.CreateUser;

public record CreateUserCommand(
    UserType UserType,
    string FirstName,
    string LastName,
    [EmailAddress] string Email,
    string Password,
    string ConfirmPassword,
    [Phone] string? PhoneNumber = null) : ICommand<CreateUserResponse>
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