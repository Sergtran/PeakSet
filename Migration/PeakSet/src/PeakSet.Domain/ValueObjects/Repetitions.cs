using PeakSet.Domain.Common;

namespace PeakSet.Domain.ValueObjects;

public sealed class Repetitions : ValueObject
{
	public int Value { get; }

	private Repetitions()
	{
	}

	public Repetitions(int value)
	{
		if (value < 0)
			throw new ArgumentOutOfRangeException(nameof(value), "Repetitions cannot be negative.");

		Value = value;
	}

	protected override IEnumerable<object?> GetEqualityComponents()
	{
		yield return Value;
	}

	public override string ToString()
	{
		return Value.ToString();
	}

	public static implicit operator int(Repetitions repetitions)
	{
		return repetitions.Value;
	}
}