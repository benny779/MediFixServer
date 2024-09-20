using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Extensions;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Categories.GetCategories;

internal sealed class GetCategoriesRequestHandler(
    ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoriesRequest, CategoriesResponse>
{
    public async Task<Result<CategoriesResponse>> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        var categories = request.WithInactive ?
            await categoryRepository.GetAllAsync(cancellationToken) :
            await categoryRepository
                .GetQueryable()
                .Where(c => c.IsActive)
                .ResultToListAsync(cancellationToken);

        if (categories.IsFailure)
        {
            return categories.Error;
        }

        return CategoriesResponse.FromDomainCategories(
            categories.Value
                .OrderBy(cat => cat.Name));
    }
}