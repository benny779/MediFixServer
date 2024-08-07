using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Categories.GetCategory;

public record GetCategoryRequest(Guid Id) : IQuery<CategoryResponse>;