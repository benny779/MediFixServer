namespace MediFix.Application.Abstractions.Services;

public interface ICurrentUser
{
    Guid Id { get; }
    string FirstName { get; }
    string LastName { get; }
    string FullName { get; }
    string Email { get; }
    string? PhoneNumber { get; }
}
