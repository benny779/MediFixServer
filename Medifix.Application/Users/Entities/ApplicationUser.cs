using MediFix.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace MediFix.Application.Users.Entities;

public sealed class ApplicationUser : IdentityUser, IDomainUser
{
    public const int FirstNameMaxLength = 30;
    public const int LastNameMaxLength = 30;

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;


    private ApplicationUser()
    {
    }

    private ApplicationUser(string userName)
        : base(userName)
    {
    }

    public ApplicationUser(string firstName, string lastName, string email)
        : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public ApplicationUser(string userName, string firstName, string lastName, string email)
        : this(userName)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}
