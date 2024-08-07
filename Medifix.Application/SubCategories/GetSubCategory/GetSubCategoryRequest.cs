using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.SubCategories.GetSubCategory;

public record GetSubCategoryRequest(Guid Id) : IQuery<SubCategoryResponse>;