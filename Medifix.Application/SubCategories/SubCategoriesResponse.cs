using MediFix.Domain.Categories;

namespace MediFix.Application.SubCategories;

public record SubCategoriesResponse(
    IEnumerable<SubCategoryResponse> SubCategories)
{
    public static SubCategoriesResponse FromDomainCategories(IEnumerable<SubCategory> subCategories)
    {
        return new SubCategoriesResponse(
            subCategories.Select(SubCategoryResponse.FromDomainSubCategory));
    }
}