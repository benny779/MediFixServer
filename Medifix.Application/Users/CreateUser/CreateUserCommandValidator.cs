using FluentValidation;
using MediFix.Application.Extensions.Validation;

namespace MediFix.Application.Users.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(user => user.UserType)
            .IsInEnum();

        RuleFor(user => user.FirstName)
            .NotEmpty()
            .MinimumLength(2);

        RuleFor(user => user.LastName)
            .NotEmpty()
            .MinimumLength(2);

        RuleFor(user => user.Email)
            .EmailAddress();

        RuleFor(user => user.PhoneNumber)
            .PhoneNumber();

        RuleFor(user => user.Password)
            .ConfirmPassword(user => user.ConfirmPassword);
    }
}