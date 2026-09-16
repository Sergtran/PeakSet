using PeakSet.Domain.Common;

namespace PeakSet.Domain.ValueObjects;

public sealed class Weight : ValueObject
{
	public decimal Value { get; }

	private Weight()
	{
	}

	public Weight(decimal value)
	{
		if (value < 0)
			throw new ArgumentOutOfRangeException(nameof(value), "Weight cannot be negative.");

		Value = value;
	}

	protected override IEnumerable<object?> GetEqualityComponents()
	{
		yield return Value;
	}

	public override string ToString()
	{
		return $"{Value} kg";
	}

	public static implicit operator decimal(Weight weight)
	{
		return weight.Value;
	}
}