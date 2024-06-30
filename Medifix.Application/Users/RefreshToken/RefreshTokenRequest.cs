using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.RefreshToken;

public record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken) : ICommand<RefreshTokenResponse>;