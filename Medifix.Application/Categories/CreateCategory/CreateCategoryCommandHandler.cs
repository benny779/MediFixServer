using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Categories.CreateCategory;

internal sealed class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateCategoryCommand, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await categoryRepository.ExistsAsync(c => c.Name == request.Name, cancellationToken))
        {
            return Error.AlreadyExists<Category>(nameof(Category.Name));
        }

        var categoryId = CategoryId.Create();

        var category = new Category(categoryId, request.Name, request.IsActive);

        categoryRepository.Insert(category);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CategoryResponse.FromDomainCategory(category);
    }
}