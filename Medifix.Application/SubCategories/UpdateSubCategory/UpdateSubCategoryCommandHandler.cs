using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.SubCategories.UpdateSubCategory;

internal sealed class UpdateSubCategoryCommandHandler(
    ISubCategoryRepository subCategoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateSubCategoryCommand>
{
    public async Task<Result> Handle(UpdateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategoryId = SubCategoryId.From(request.Id);

        var subCategoryResult = await subCategoryRepository.GetByIdAsync(subCategoryId, cancellationToken);

        if (subCategoryResult.IsFailure)
        {
            return subCategoryResult.Error;
        }

        var subCategory = subCategoryResult.Value;

        if (await SubCategoryWithSameNameAndCategoryExistsAsync(subCategory, cancellationToken))
        {
            return Error.AlreadyExists<SubCategory>(nameof(SubCategory.Name));
        }

        subCategory.Name = request.Name;
        subCategory.IsActive = request.IsActive;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private Task<bool> SubCategoryWithSameNameAndCategoryExistsAsync(SubCategory subCategory, CancellationToken cancellationToken)
    {
        return subCategoryRepository.ExistsAsync(
            s => s.Name == subCategory.Name
                 && s.CategoryId == subCategory.CategoryId
                 && s.Id != subCategory.Id,
            cancellationToken);
    }
}