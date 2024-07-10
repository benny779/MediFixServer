using MediFix.Domain.Expertises;

namespace MediFix.Domain.Users;

public sealed class Practitioner : AggregateRoot<PractitionerId>
{
    private readonly HashSet<Expertise> _expertises = [];

    public IReadOnlyList<Expertise> Expertises => _expertises.ToList();


    private Practitioner(PractitionerId id) : base(id)
    {
    }

    public static Result<Practitioner> Create(PractitionerId practitionerId)
    {
        return new Practitioner(practitionerId);
    }

    public bool AddExpertise(Expertise? expertise)
    {
        return expertise is not null && _expertises.Add(expertise);
    }

    public bool RemoveExpertise(Expertise? expertise)
    {
        return expertise is not null && _expertises.Remove(expertise);
    }
}