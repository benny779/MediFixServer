﻿using MediFix.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace MediFix.Application.Users.Entities;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public const int FirstNameMaxLength = 30;
    public const int LastNameMaxLength = 30;

    public UserType Type { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string FullName => $"{LastName} {FirstName}";

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenValidity { get; set; }

    public ApplicationUser()
    {
        Id = Guid.NewGuid();
        SecurityStamp = Guid.NewGuid().ToString();
    }
}