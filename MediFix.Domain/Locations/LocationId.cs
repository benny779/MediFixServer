namespace MediFix.Domain.Locations;

public record LocationId : StronglyTypedId<Guid>
{
    private LocationId(Guid value) : base(value)
    {
    }

    public static LocationId Create() => new(Guid.NewGuid());
    public static LocationId From(Guid value) => new(value);
}