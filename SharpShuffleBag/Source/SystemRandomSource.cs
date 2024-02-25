namespace SharpShuffleBag
{
	using System;

	/// <summary>
	/// Uses <see cref="System.Random" /> as a source of randomness.
	/// </summary>
	public sealed class SystemRandomSource : IRandomRangeSource, IRandomValueSource
	{
		private readonly Random random;

		public SystemRandomSource()
		{
			random = new Random();
		}

		public SystemRandomSource(int seed)
		{
			random = new Random(seed);
		}

		public int Range(int minInclusive, int maxExclusive)
		{
			return random.Next(minInclusive, maxExclusive);
		}

		public float Value => random.Next();
	}
}