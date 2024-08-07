using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Expertises;
using MediFix.Domain.Categories;
using MediFix.Domain.Expertises;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Categories.DeleteCategoryExpertise;

internal sealed class DeleteCategoryExpertiseCommandHandler(
    ICategoryRepository categoryRepository,
    IExpertiseRepository expertiseRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteCategoryExpertiseCommand>
{
    public async Task<Result> Handle(DeleteCategoryExpertiseCommand request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.CategoryId);
        var expertiseId = ExpertiseId.From(request.ExpertiseId);

        var categoryResult = await categoryRepository
            .GetByIdWithNavigationAsync(categoryId, cancellationToken);

        if (categoryResult.IsFailure)
        {
            return categoryResult.Error;
        }

        var expertiseResult = await expertiseRepository
            .GetByIdWithNavigationAsync(expertiseId, cancellationToken);

        if (expertiseResult.IsFailure)
        {
            return expertiseResult.Error;
        }

        if (categoryResult.Value.DeleteExpertise(expertiseResult.Value))
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}