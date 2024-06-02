namespace MediFix.Domain.Users;

public record ExpertiseId : StronglyTypedId<Guid>
{
    private ExpertiseId(Guid value) : base(value)
    {
    }

    public static ExpertiseId Create() => new(Guid.NewGuid());
    public static ExpertiseId From(Guid value) => new(value);
}