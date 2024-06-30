namespace MediFix.Application.Users.Entities;

public record AuthenticationResponse(
    string AccessToken,
    string RefreshToken);