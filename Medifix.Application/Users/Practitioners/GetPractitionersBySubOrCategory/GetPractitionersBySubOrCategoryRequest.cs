using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.Practitioners.GetPractitionersBySubOrCategory;

public record GetPractitionersBySubOrCategoryRequest(
    Guid? CategoryId,
    Guid? SubCategoryId)
    : IQuery<GetPractitionersBySubOrCategoryResponse>;