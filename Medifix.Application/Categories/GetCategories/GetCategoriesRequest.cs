using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Categories.GetCategories;

public record GetCategoriesRequest(bool WithInactive) : IQuery<CategoriesResponse>;