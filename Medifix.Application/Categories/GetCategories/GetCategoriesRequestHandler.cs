using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Categories.GetCategories;

internal sealed class GetCategoriesRequestHandler(
    ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoriesRequest, CategoriesResponse>
{
    public async Task<Result<CategoriesResponse>> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);

        if (categories.IsFailure)
        {
            return categories.Error;
        }

        return CategoriesResponse.FromDomainCategories(categories.Value!);
    }
}