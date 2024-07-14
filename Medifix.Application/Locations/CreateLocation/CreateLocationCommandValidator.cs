using FluentValidation;

namespace MediFix.Application.Locations.CreateLocation;

public class CreateLocationCommandValidator 
    : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        RuleFor(location => location.Name)
            .NotEmpty();

        RuleFor(location => location.LocationType)
            .IsInEnum();
    }
}
