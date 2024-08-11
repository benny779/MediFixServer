using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;

namespace MediFix.Application.Categories;

public record CategoriesResponse(IEnumerable<CategoryResponse> Items) : IListResponse<CategoryResponse>
{
    internal static CategoriesResponse FromDomainCategories(IEnumerable<Category> categories)
    {
        return new CategoriesResponse(
            categories
                .Select(CategoryResponse.FromDomainCategory)
                .ToList());
    }
}