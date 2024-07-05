namespace MediFix.Domain.Users;

public sealed class Practitioner : AggregateRoot<PractitionerId>
{
    private readonly List<Expertise> _expertises = [];

    public IReadOnlyList<Expertise> Expertises => _expertises.AsReadOnly();


    private Practitioner(PractitionerId id) : base(id)
    {
    }

    public static Result<Practitioner> Create(PractitionerId practitionerId)
    {
        return new Practitioner(practitionerId);
    }

    public bool AddExpertise(Expertise? expertise)
    {
        if (expertise is null)
        {
            return false;
        }

        if (_expertises.Exists(e => e == expertise))
        {
            return false;
        }

        _expertises.Add(expertise);

        return true;
    }

    public bool RemoveExpertise(Expertise? expertise)
        => expertise is not null && _expertises.Remove(expertise);
}