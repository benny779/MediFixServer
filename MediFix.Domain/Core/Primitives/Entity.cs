namespace MediFix.Domain.Core.Primitives;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
{
    const int HashMultiplier = 37;

    public TId Id { get; init; }

    protected Entity(TId id)
    {
        Id = id;
    }

#pragma warning disable CS8618
    protected Entity()
    {
    }
#pragma warning restore CS8618

    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
            return false;

        if (other.GetType() != GetType())
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        return Equals((Entity<TId>)obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id!) * HashMultiplier;
    }

    public static bool operator ==(Entity<TId> first, Entity<TId> second)
    {
        return Equals(first, second);
    }

    public static bool operator !=(Entity<TId> first, Entity<TId> second)
    {
        return !(first == second);
    }
}

public abstract class EventEntity<TId> : Entity<TId>
{
    private readonly List<Event> _domainEvents = [];

    protected EventEntity(TId id) : base(id)
    {
    }

    public void AddDomainEvent(Event domainEvent)
        => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(Event domainEvent)
        => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}