using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;

namespace MediFix.Application.SubCategories;

public record SubCategoryResponse(
    Guid Id, 
    string Name, 
    bool IsActive) : ICreatedResponse
{
    public static SubCategoryResponse FromDomainSubCategory(SubCategory subCategory)
    {
        return new(subCategory.Id, subCategory.Name, subCategory.IsActive);
    }
}