using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.SubCategories.GetSubCategory;

internal sealed class GetSubCategoryRequestHandler(
    ISubCategoryRepository subCategoryRepository)
    : IQueryHandler<GetSubCategoryRequest, SubCategoryResponse>
{
    public async Task<Result<SubCategoryResponse>> Handle(GetSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var subCategoryId = SubCategoryId.From(request.Id);

        var categoryResult = await subCategoryRepository.GetByIdAsync(subCategoryId, cancellationToken);

        if (categoryResult.IsFailure)
        {
            return categoryResult.Error;
        }

        return SubCategoryResponse.FromDomainSubCategory(categoryResult.Value);
    }
}