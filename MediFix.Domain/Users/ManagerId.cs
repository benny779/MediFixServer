namespace MediFix.Domain.Users;

public record ManagerId : StronglyTypedId<Guid>
{
    private ManagerId(Guid value) : base(value)
    {
    }

    public static ManagerId Create() => new(Guid.NewGuid());
    public static ManagerId From(Guid value) => new(value);
}