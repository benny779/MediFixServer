namespace MediFix.Domain.Users;

public record ClientId : StronglyTypedId<Guid>
{
    private ClientId(Guid value) : base(value)
    {
    }

    public static ClientId Create() => new(Guid.NewGuid());
    public static ClientId From(Guid value) => new(value);
}