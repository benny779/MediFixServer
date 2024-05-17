namespace MediFix.Domain.Users;

public interface IDomainUser
{
}

public sealed class User : AggregateRoot<UserId>, IDomainUser
{
    private User()
    {
    }

    public User(UserId id) : base(id)
    {
    }
}