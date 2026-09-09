namespace PeakSet.Domain.Common;

public abstract class Entity : IEquatable<Entity>
{
	public Guid Id { get; protected set; }

	protected Entity()
	{
	}

	protected Entity(Guid id)
	{
		Id = id;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as Entity);
	}

	public bool Equals(Entity? other)
	{
		if (other is null)
			return false;

		if (ReferenceEquals(this, other))
			return true;

		if (GetType() != other.GetType())
			return false;

		return Id == other.Id;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(GetType(), Id);
	}

	public static bool operator ==(Entity? left, Entity? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(Entity? left, Entity? right)
	{
		return !Equals(left, right);
	}
}