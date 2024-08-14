using FluentValidation;

namespace MediFix.Application.ServiceCalls.CloseServiceCallCommand;

public class CloseServiceCallCommandValidator : AbstractValidator<CloseServiceCallCommand>
{
    public CloseServiceCallCommandValidator()
    {
        RuleFor(x => x.CloseDetails)
            .NotEmpty();
    }
}
