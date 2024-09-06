using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Categories;
using MediFix.Domain.Categories;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.SubCategories.CreateSubCategory;

internal sealed class CreateSubCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ISubCategoryRepository subCategoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateSubCategoryCommand, SubCategoryResponse>
{
    public async Task<Result<SubCategoryResponse>> Handle(CreateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.CategoryId);
        if (!await categoryRepository.ExistsAsync(c => c.Id == categoryId, cancellationToken))
        {
            return Error.EntityNotFound<Category>(categoryId.Value);
        }

        if (await subCategoryRepository.ExistsAsync(s => s.Name == request.Name, cancellationToken))
        {
            return Error.AlreadyExists<SubCategory>(nameof(SubCategory.Name));
        }

        var subCategoryId = SubCategoryId.Create();

        var subCategory = new SubCategory(subCategoryId, request.Name, categoryId, request.IsActive);

        subCategoryRepository.Insert(subCategory);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return SubCategoryResponse.FromDomainSubCategory(subCategory);
    }
}