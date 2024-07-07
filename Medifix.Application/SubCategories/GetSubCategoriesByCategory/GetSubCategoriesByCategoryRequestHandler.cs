using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Categories;
using MediFix.Domain.Categories;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.SubCategories.GetSubCategoriesByCategory;

internal sealed class GetSubCategoriesByCategoryRequestHandler(
    ISubCategoryRepository subCategoryRepository)
    : IQueryHandler<GetSubCategoriesByCategoryRequest, SubCategoriesResponse>
{
    public async Task<Result<SubCategoriesResponse>> Handle(GetSubCategoriesByCategoryRequest request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.CategoryId);

        var subCategories = await subCategoryRepository
            .GetQueryable()
            .Where(sc => sc.CategoryId == categoryId)
            .ToListAsync(cancellationToken);

        if (!subCategories.Any())
        {
            return Error.NotFound(
                "SubCategories.ByCategory.NotFound",
                $"No SubCategories found for the category with the id '{request.CategoryId}'.");
        }

        return SubCategoriesResponse.FromDomainCategories(
            subCategories
                .OrderBy(sc => sc.Name));
    }
}