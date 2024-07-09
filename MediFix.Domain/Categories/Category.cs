using MediFix.Domain.Expertises;

namespace MediFix.Domain.Categories;

public class Category(CategoryId id, string name) : Entity<CategoryId>(id)
{
    public const int NameMaxLength = 30;

    private readonly List<Expertise> _allowedExpertises = [];


    public string Name { get; set; } = name;
    
    public IReadOnlyList<Expertise> AllowedExpertises => _allowedExpertises.AsReadOnly();

    
    public bool AddExpertise(Expertise? expertise)
    {
        if (expertise is null)
        {
            return false;
        }

        if (_allowedExpertises.Exists(e => e == expertise))
        {
            return false;
        }

        _allowedExpertises.Add(expertise);

        return true;
    }
}