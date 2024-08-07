using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Categories.UpdateCategory;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    bool IsActive) : ICommand;