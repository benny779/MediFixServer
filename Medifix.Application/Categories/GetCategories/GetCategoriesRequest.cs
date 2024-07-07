using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Categories.GetCategories;

public record GetCategoriesRequest : IQuery<CategoriesResponse>;