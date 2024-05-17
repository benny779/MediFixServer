namespace MediFix.Domain.Users;

public sealed class Practitioner : AggregateRoot<PractitionerId>
{
    public UserId UserId { get; private set; } = null!;

    private readonly HashSet<Expertise> _expertises = [];
    public IReadOnlyList<Expertise> Expertises => [.. _expertises];


    private Practitioner(PractitionerId id) : base(id)
    {
    }

    public static Result<Practitioner> Create(
        PractitionerId practitionerId,
        UserId userId
    )
    {
        return new Practitioner(practitionerId)
        {
            UserId = userId
        };
    }

    public bool AddExpertise(Expertise? expertise)
        => expertise is not null && _expertises.Add(expertise);

    public bool RemoveExpertise(Expertise? expertise)
        => expertise is not null && _expertises.Remove(expertise);
}