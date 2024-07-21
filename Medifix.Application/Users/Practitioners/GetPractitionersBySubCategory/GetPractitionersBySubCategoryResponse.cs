namespace MediFix.Application.Users.Practitioners.GetPractitionersBySubCategory;

public record GetPractitionersBySubCategoryResponse(
    IEnumerable<PractitionerWithServiceCallCount> Practitioners);