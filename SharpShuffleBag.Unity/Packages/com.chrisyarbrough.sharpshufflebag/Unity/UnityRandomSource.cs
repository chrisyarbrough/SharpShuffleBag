namespace SharpShuffleBag.Unity
{
	using UnityEngine;

	/// <summary>
	/// Uses <see cref="UnityEngine.Random"/> as the source of randomness.
	/// Call <see cref="UnityEngine.Random.InitState"/> to set the seed globally.
	/// </summary>
	public sealed class UnityRandomSource : IRandomRangeSource, IRandomValueSource
	{
		public int Range(int minInclusive, int maxExclusive)
		{
			return Random.Range(minInclusive, maxExclusive);
		}

		public float Value => Random.value;
	}
}