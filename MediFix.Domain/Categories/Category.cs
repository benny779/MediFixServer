using MediFix.Domain.Expertises;
using MediFix.Domain.Users;

namespace MediFix.Domain.Categories;

public class Category(CategoryId id, string name, bool isActive = true) : Entity<CategoryId>(id)
{
    public const int NameMaxLength = 30;

    private readonly HashSet<Expertise> _allowedExpertises = [];


    public string Name { get; set; } = name;
    public bool IsActive { get; set; } = isActive;

    public IReadOnlySet<Expertise> AllowedExpertises => _allowedExpertises;


    public bool AddExpertise(Expertise? expertise)
    {
        return expertise is not null && _allowedExpertises.Add(expertise);
    }

    public bool DeleteExpertise(Expertise? expertise)
    {
        return expertise is not null && _allowedExpertises.Remove(expertise);
    }

    public bool IsPractitionerAllowed(Practitioner practitioner)
    {
        return _allowedExpertises
            .Intersect(practitioner.Expertises)
            .Any();
    }
}