// ReSharper disable UnusedMember.Global

namespace SharpShuffleBag
{
	using System;

	/// <summary>
	/// Produces a random int within a range of [minInclusive..maxExclusive].
	/// </summary>
	/// <remarks>
	/// This abstraction can be used to replace the default random number generator
	/// with a deterministic implementation (e.g. with a seed or for unit testing).
	/// </remarks>
	public interface IRandomRangeSource
	{
		int Range(int minInclusive, int maxExclusive);

		static IRandomRangeSource Default
		{
			get => defaultSource;
			set => defaultSource = value ?? throw new ArgumentNullException(nameof(value));
		}

		private static IRandomRangeSource defaultSource = new SystemRandomSource();
	}
}