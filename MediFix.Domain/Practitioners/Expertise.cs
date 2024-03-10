using MediFix.Domain.Core.Primitives;

namespace MediFix.Domain.Practitioners;

public class Expertise(ExpertiseId id, string name) : Entity<ExpertiseId>(id)
{
    public string Name { get; set; } = name;
}