using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Categories.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.Id);

        var categoryResult = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

        if (categoryResult.IsFailure)
        {
            return categoryResult.Error;
        }

        var category = categoryResult.Value;

        if (await CategoryWithSameNameExistsAsync(category, cancellationToken))
        {
            return Error.AlreadyExists<Category>(nameof(Category.Name));
        }
        
        category.Name = request.Name;
        category.IsActive = request.IsActive;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private Task<bool> CategoryWithSameNameExistsAsync(Category category, CancellationToken cancellationToken)
    {
        return categoryRepository.ExistsAsync(
            c => c.Name == category.Name
                 && c.Id != category.Id,
            cancellationToken);
    }
}