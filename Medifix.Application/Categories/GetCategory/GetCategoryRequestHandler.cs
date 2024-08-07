using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Categories.GetCategory;

internal sealed class GetCategoryRequestHandler(
    ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoryRequest, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.Id);

        var categoryResult = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

        if (categoryResult.IsFailure)
        {
            return categoryResult.Error;
        }

        return CategoryResponse.FromDomainCategory(categoryResult.Value);
    }
}