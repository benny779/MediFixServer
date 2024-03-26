namespace MediFix.Domain.Users;

public record UserId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static UserId Create() => new (Guid.NewGuid());
}