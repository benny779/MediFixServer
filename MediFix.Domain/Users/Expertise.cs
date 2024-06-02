namespace MediFix.Domain.Users;

public sealed class Expertise(ExpertiseId id, string name) : Entity<ExpertiseId>(id)
{
    public const int NameMaxLength = 32;

    public string Name { get; set; } = name;


    public IReadOnlyList<Practitioner> Practitioners { get; set; } = [];
}