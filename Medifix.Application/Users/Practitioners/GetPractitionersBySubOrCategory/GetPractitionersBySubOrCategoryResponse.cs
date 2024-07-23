namespace MediFix.Application.Users.Practitioners.GetPractitionersBySubOrCategory;

public record GetPractitionersBySubOrCategoryResponse(
    IEnumerable<PractitionerWithServiceCallCount> Practitioners);