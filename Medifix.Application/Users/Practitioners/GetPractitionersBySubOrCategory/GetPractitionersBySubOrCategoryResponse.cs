using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.Practitioners.GetPractitionersBySubOrCategory;

public record GetPractitionersBySubOrCategoryResponse(
    IEnumerable<PractitionerWithServiceCallCount> Items) : IListResponse<PractitionerWithServiceCallCount>;