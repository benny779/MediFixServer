using MediFix.Domain.ServiceCalls;

namespace MediFix.Domain.Users;

public class User : Entity<UserId>
{
    public const int FirstNameMaxLength = 30;
    public const int LastNameMaxLength = 30;

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string HashedPassword { get; private set; } = null!;

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