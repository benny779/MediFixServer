namespace MediFix.Domain.Practitioners;

public class Expertise(ExpertiseId id, string name) : Entity<ExpertiseId>(id)
{
    public const int NameMaxLength = 32;

    public string Name { get; set; } = name;
}