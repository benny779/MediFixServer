namespace MediFix.Domain.Users;

public class Expertise(ExpertiseId id, string name) : Entity<ExpertiseId>(id)
{
    public const int NameMaxLength = 32;

    public string Name { get; set; } = name;


    private readonly List<Practitioner> _practitioners = [];
    public IReadOnlyList<Practitioner> Practitioners => _practitioners;
}