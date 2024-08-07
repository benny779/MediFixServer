using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.SubCategories.UpdateSubCategory;

public record UpdateSubCategoryCommand(
    Guid Id,
    string Name,
    bool IsActive) : ICommand;