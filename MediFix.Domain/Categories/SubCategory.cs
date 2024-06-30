namespace MediFix.Domain.Categories;

public class SubCategory(SubCategoryId id, string name, CategoryId categoryId) : Entity<SubCategoryId>(id)
{
    public const int NameMaxLength = 50;

    public string Name { get; set; } = name;

    public CategoryId CategoryId { get; set; } = categoryId;

    public Category Category { get; }
}