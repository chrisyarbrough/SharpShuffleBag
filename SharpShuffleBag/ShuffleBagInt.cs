// ReSharper disable UnusedMember.Global

namespace SharpShuffleBag
{
	using System.Collections.Generic;

	/// <summary>
	/// A common type of <see cref="ShuffleBag{T}" /> based on int values
	/// used as indices into another collection.
	/// </summary>
	public sealed class ShuffleBag : ShuffleBag<int>
	{
		public ShuffleBag()
		{
		}

		public ShuffleBag(int fixedCount) : base(fixedCount)
		{
			for (int i = 0; i < fixedCount; i++)
				Add(i);
		}

		public ShuffleBag(IEnumerable<int> items) : base(items)
		{
		}
	}
}