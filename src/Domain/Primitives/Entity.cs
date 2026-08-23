namespace Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; private init;}

    protected Entity() => Id = Guid.CreateVersion7();

    protected Entity(Guid id) => Id = id;

    public override bool Equals(object? obj) => 
        obj is Entity other && GetType() == other.GetType() && Id == other.Id;

    public bool Equals(Entity? other) =>
        other is not null && GetType() == other.GetType() && Id == other.Id;

    public override int GetHashCode() => Id.GetHashCode() * 41;

    public static bool operator ==(Entity? first, Entity? second) =>
        first is not null && second is not null && first.Equals(second);
    
    public static bool operator !=(Entity? first, Entity? second) =>
        !(first == second);
}