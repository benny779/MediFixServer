namespace MediFix.Domain.Locations;

public record LocationId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static LocationId Create() => new(Guid.NewGuid());
}