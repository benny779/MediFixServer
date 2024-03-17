using MediFix.Domain.Core.Primitives;
using MediFix.SharedKernel.Results;

namespace MediFix.Domain.Practitioners;

public class Practitioner : Entity<PractitionerId>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public ExpertiseId? ExpertiseId { get; private set; }


    private Practitioner(PractitionerId id) : base(id)
    {
    }

    public static Result<Practitioner> Create(
        PractitionerId practitionerId,
        string firstName,
        string lastName,
        string email,
        string phone,
        ExpertiseId expertiseId
    )
    {
        return new Practitioner(practitionerId)
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            ExpertiseId = expertiseId
        };
    }
}