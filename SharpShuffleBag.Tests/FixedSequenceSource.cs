namespace SharpShuffleBag.Tests;

/// <summary>
/// A random source which forces the shuffle bag to return a fixed sequence of values from min to max.
/// </summary>
public class FixedSequenceSource : IRandomRangeSource
{
	private readonly int[] sequence;

	public FixedSequenceSource(params int[] sequence)
	{
		this.sequence = sequence;
	}

	public int Range(int minInclusive, int maxExclusive) => sequence[minInclusive];
}