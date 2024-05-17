namespace MediFix.Domain.Categories;

public record CategoryId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static CategoryId Create() => new(Guid.NewGuid());
}