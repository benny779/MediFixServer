using MediFix.Domain.Categories;
using MediFix.Domain.Users;

namespace MediFix.Domain.Expertises;

public sealed class Expertise(ExpertiseId id, string name) : Entity<ExpertiseId>(id)
{
    public const int NameMaxLength = 32;

    private readonly List<Practitioner> _practitioners = [];
    private readonly List<Category> _categories = [];

    public string Name { get; set; } = name;


    public IReadOnlyList<Practitioner> Practitioners => _practitioners.AsReadOnly();
    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();
}