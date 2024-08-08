using MediFix.Application.Expertises;

namespace MediFix.Application.Users.Practitioners;

public record PractitionerResponse(
    Guid PractitionerId,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string? PhoneNumber,
    IEnumerable<ExpertiseResponse> Expertises);