namespace MediFix.Domain.Categories;

public class Category(CategoryId id, string name) : Entity<CategoryId>(id)
{
    public const int NameMaxLength = 30;

    public string Name { get; set; } = name;
}