namespace MediFix.Domain.Users;

public sealed class Manager : AggregateRoot<ManagerId>
{
    public UserId UserId { get; private set; } = null!;


    private Manager(ManagerId id) : base(id)
    {
    }

    public static Result<Manager> Create(
        ManagerId practitionerId,
        UserId userId
    )
    {
        return  new Manager(practitionerId)
        {
            UserId = userId
        };
    }
}