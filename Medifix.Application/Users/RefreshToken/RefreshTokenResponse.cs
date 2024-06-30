namespace MediFix.Application.Users.RefreshToken;

public record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken);