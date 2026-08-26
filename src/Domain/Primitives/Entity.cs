namespace Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; private init; }

    // EF Core could use this, keep this and see.
    // protected Entity() => Id = Guid.CreateVersion7();

    protected Entity(Guid id) => Id = id;

    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || (obj is Entity other && GetType() == other.GetType() && Id == other.Id);

    public bool Equals(Entity? other) =>
        ReferenceEquals(this, other) || (other is not null && GetType() == other.GetType() && Id == other.Id);

    public override int GetHashCode() => Id.GetHashCode() * 41;

    public static bool operator ==(Entity? first, Entity? second) =>
        ReferenceEquals(first, second) || (first is not null && first.Equals(second));

    public static bool operator !=(Entity? first, Entity? second) =>
        !(first == second);
}