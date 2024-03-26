namespace MediFix.Domain.Practitioners;

public class Practitioner : AggregateRoot<PractitionerId>
{
    public const int FirstNameMaxLength = 30;
    public const int LastNameMaxLength = 30;

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
        ExpertiseId? expertiseId = null
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

    public void SetExpertise(ExpertiseId expertiseId) =>
        ExpertiseId = expertiseId;
}