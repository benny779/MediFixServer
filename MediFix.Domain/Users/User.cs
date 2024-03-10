using MediFix.Domain.Core.Primitives;
using MediFix.SharedKernel.Results;

namespace MediFix.Domain.Users;

public class User : Entity<UserId>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string HashedPassword { get; private set; }

    private User(UserId id) : base(id)
    {
    }

    public static Result<User> Create(
        UserId id,
        string firstName,
        string lastName,
        string email,
        string phone,
        string hashedPassword)
    {
        return new User(id)
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            HashedPassword = hashedPassword
        };
    }
}