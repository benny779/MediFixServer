namespace MediFix.Domain.Users;

public record PractitionerId : StronglyTypedId<Guid>
{
    private PractitionerId(Guid value) : base(value)
    {
    }

    public static PractitionerId Create() => new(Guid.NewGuid());
    public static PractitionerId From(Guid value) => new(value);
}