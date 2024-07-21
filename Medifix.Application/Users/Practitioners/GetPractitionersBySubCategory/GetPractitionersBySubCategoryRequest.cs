using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.Practitioners.GetPractitionersBySubCategory;

public record GetPractitionersBySubCategoryRequest(Guid SubCategoryId)
    : IQuery<GetPractitionersBySubCategoryResponse>;