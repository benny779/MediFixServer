using MediFix.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MediFix.Infrastructure.Authentication;

internal sealed class CurrentUser : ICurrentUser
{
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string FullName => $"{LastName} {FirstName}";
    public string Email { get; }
    public string? PhoneNumber { get; }

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User
                   ?? throw new UnauthorizedAccessException("Cannot identify the logged-in user.");

        Id = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? throw new UnauthorizedAccessException("User ID is missing."));

        FirstName = user.FindFirstValue(ClaimTypes.GivenName)
                    ?? throw new UnauthorizedAccessException("First name is missing.");

        LastName = user.FindFirstValue(ClaimTypes.Surname)
                   ?? throw new UnauthorizedAccessException("Last name is missing.");

        Email = user.FindFirstValue(ClaimTypes.Email)
                ?? throw new UnauthorizedAccessException("Email is missing.");

        PhoneNumber = user.FindFirstValue("phone");
    }
}
