// ReSharper disable UnusedMember.Global

namespace SharpShuffleBag
{
	/// <summary>
	/// Produces a random float value.
	/// </summary>
	public interface IRandomValueSource
	{
		float Value { get; }
	}
}