namespace MediFix.Domain.Core.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetEqualityComponents();

    protected static object GetEqualityStringComponents(string value)
        => StringComparer.OrdinalIgnoreCase.GetHashCode(value);


    public override bool Equals(object? obj)
    {
        return obj is ValueObject other && ValuesAreEqual(other);
    }

    public bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(
                default(int),
                (hashcode, value) =>
                    HashCode.Combine(hashcode, value.GetHashCode()));
    }

    private bool ValuesAreEqual(ValueObject other)
    {
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
        => !(left == right);
}


public abstract class SingleValueObject<TValue> : ValueObject
    where TValue : notnull
{
    public TValue Value { get; init; }

    protected internal SingleValueObject(TValue value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
        => Value.ToString()!;
}

public abstract class SingleValueObject(string value) : SingleValueObject<string>(value)
{
    public static implicit operator string(SingleValueObject valueObject) => valueObject.Value;
    //public static implicit operator SingleValueObject?(string? value) => Create(value);
}
