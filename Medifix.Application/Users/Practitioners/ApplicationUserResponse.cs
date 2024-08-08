using MediFix.Domain.Users;

namespace MediFix.Application.Users.Practitioners;

public record ApplicationUserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    UserType Type);