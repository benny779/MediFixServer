namespace MediFix.Application.Users.CreateUser;

public record CreateUserResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone);