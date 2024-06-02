namespace MediFix.Application.Users.Entities;

public static class ApplicationUserExtensions
{
    public static bool IsRefreshTokenValid(this ApplicationUser user, string refreshToken)
        => user.RefreshToken == refreshToken
           && user.RefreshTokenValidity > DateTime.Now;
}