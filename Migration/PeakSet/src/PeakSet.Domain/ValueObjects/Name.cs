using PeakSet.Domain.Common;

namespace PeakSet.Domain.ValueObjects;

public sealed class Name : ValueObject
{
	public string Value { get; }

	private Name()
	{
		Value = string.Empty;
	}

	public Name(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value);

		Value = value.Trim();
	}

	protected override IEnumerable<object?> GetEqualityComponents()
	{
		yield return Value;
	}

	public override string ToString()
	{
		return Value;
	}

	public static implicit operator string(Name name)
	{
		return name.Value;
	}
}