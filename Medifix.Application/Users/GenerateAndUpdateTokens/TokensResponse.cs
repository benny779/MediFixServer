namespace MediFix.Application.Users.GenerateAndUpdateTokens;

internal record TokensResponse(
    string AccessToken,
    string RefreshToken);