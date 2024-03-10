using MediFix.Domain.Core.Primitives;

namespace MediFix.Domain.Categories;

public class SubCategory(SubCategoryId id, string name, CategoryId parentId) : Entity<SubCategoryId>(id)
{
    public string Name { get; set; } = name;

    public CategoryId ParentId { get; set; } = parentId;
}