namespace MediFix.Application.Users.Clients;

public record ClientResponse(
    Guid ClientId,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string? PhoneNumber);