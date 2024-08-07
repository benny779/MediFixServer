using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Categories;
using MediFix.Domain.Categories;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.SubCategories.GetSubCategories;

internal class GetSubCategoriesRequestHandler(
    ICategoryRepository categoryRepository,
    ISubCategoryRepository subCategoryRepository)
    : IQueryHandler<GetSubCategoriesRequest, SubCategoriesResponse>
{
    public async Task<Result<SubCategoriesResponse>> Handle(GetSubCategoriesRequest request, CancellationToken cancellationToken)
    {
        var query = subCategoryRepository.GetQueryable();

        if (request.CategoryId.HasValue)
        {
            var categoryId = CategoryId.From(request.CategoryId.Value);

            if (!await categoryRepository.ExistsAsync(c => c.Id == categoryId, cancellationToken))
            {
                return Error.EntityNotFound<Category>(categoryId.Value);
            }

            query = query.Where(sc => sc.CategoryId == categoryId);
        }
        
        var subCategories = await query.ToListAsync(cancellationToken);

        return SubCategoriesResponse
            .FromDomainCategories(subCategories.OrderBy(cat => cat.Name));
    }
}