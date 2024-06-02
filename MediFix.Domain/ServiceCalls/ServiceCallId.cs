namespace MediFix.Domain.ServiceCalls;

public record ServiceCallId : StronglyTypedId<Guid>
{
    private ServiceCallId(Guid value) : base(value)
    {
    }

    public static ServiceCallId Create() => new(Guid.NewGuid());
    public static ServiceCallId From(Guid value) => new(value);
}