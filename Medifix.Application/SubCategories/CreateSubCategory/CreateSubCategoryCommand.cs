using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.SubCategories.CreateSubCategory;

public record CreateSubCategoryCommand(
    string Name,
    Guid CategoryId,
    bool IsActive) : ICommand<SubCategoryResponse>;