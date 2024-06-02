namespace MediFix.Domain.Categories;

public record CategoryId : StronglyTypedId<Guid>
{
    private CategoryId(Guid value) : base(value)
    {
    }

    public static CategoryId Create() => new(Guid.NewGuid());
    public static CategoryId From(Guid value) => new(value);
}