using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Categories.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    bool IsActive) : ICreateCommand<CategoryResponse>;