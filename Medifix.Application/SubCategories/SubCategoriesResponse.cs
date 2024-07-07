using MediFix.Domain.Categories;

namespace MediFix.Application.SubCategories;

public record SubCategoriesResponse(
    List<SubCategoryResponse> Categories)
{
    public static SubCategoriesResponse FromDomainCategories(IEnumerable<SubCategory> subCategories)
    {
        return new SubCategoriesResponse(
            subCategories.Select(
                    subCategory => new SubCategoryResponse(subCategory.Id, subCategory.Name))
                .ToList());
    }
}