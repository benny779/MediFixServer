using MediFix.Domain.Categories;

namespace MediFix.Application.Categories;

public record CategoriesResponse(
    List<CategoryResponse> Categories)
{
    public static CategoriesResponse FromDomainCategories(IEnumerable<Category> categories)
    {
        return new CategoriesResponse(
            categories
                .Select(CategoryResponse.FromDomainCategory)
                .ToList());
    }
}