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
		/// <summary>
		/// Returns a random integer within the specified range.
		/// If <paramref name="minInclusive"/> is equal to <paramref name="maxExclusive"/>,
		/// <paramref name="minInclusive"/> is returned.
		/// </summary>
		int Range(int minInclusive, int maxExclusive);

		/// <summary>
		/// An assertion in place of a unit test because we shouldn't try to test user generated random implementation.
		/// Fetching the user types would be slow and non-deterministic tests are undesirably anyway.
		/// </summary>
		/// <remarks>
		/// This method describes how System.Random and UnityEngine.Random behave.
		/// </remarks>
		internal sealed void Verify(int value, int min, int max)
		{
			if (value == min && min == max)
			{
				return;
			}

			if (value < min || value >= max)
			{
				throw new ArgumentOutOfRangeException(
					paramName: nameof(value),
					$"{GetType()} returned {value}. " +
					$"If min is less than max, the value must be between {min} (minInclusive) and {max} (maxExclusive). " +
					"If min and max are equal, the value must be min.");
			}
		}

		static IRandomRangeSource Default
		{
			get => defaultSource;
			internal set => defaultSource = value ?? throw new ArgumentNullException(nameof(value));
		}

		private static IRandomRangeSource defaultSource = new SystemRandomSource();
	}
}