using MediFix.Domain.Core.Primitives;

namespace MediFix.Domain.Categories;

public class Category(CategoryId id, string name) : Entity<CategoryId>(id)
{
    public string Name { get; set; } = name;
}