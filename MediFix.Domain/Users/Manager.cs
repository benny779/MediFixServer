namespace MediFix.Domain.Users;

public sealed class Manager : AggregateRoot<ManagerId>
{
    private Manager(ManagerId id) : base(id)
    {
    }

    public static Result<Manager> Create(ManagerId managerId)
    {
        return new Manager(managerId);
    }
}