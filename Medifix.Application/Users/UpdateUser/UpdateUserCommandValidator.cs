using FluentValidation;
using MediFix.Application.Extensions.Validation;

namespace MediFix.Application.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(user => user.FirstName)
            .MinimumLength(2)
            .When(user => user.FirstName is not null);

        RuleFor(user => user.LastName)
            .MinimumLength(2)
            .When(user => user.LastName is not null);

        RuleFor(user => user.PhoneNumber)
            .PhoneNumber()
            .When(user => user.PhoneNumber is not null);
    }
}