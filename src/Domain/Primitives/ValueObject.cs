namespace Domain.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetAtomicValues();

    public override bool Equals(object? obj) =>
        obj is ValueObject other && GetType() == other.GetType() && Equals(other);

    public virtual bool Equals(ValueObject? other) =>
        other is not null && (ReferenceEquals(this, other) || GetAtomicValues().SequenceEqual(other.GetAtomicValues()));

    public override int GetHashCode() =>
        GetAtomicValues().Aggregate(
            default(int),
            (hashcode, value) => 
                HashCode.Combine(hashcode, value.GetHashCode));

    public static bool operator ==(ValueObject left, ValueObject right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(ValueObject left, ValueObject right) => 
        !(left == right);
}
