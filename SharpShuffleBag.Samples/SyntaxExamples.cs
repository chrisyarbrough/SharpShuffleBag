namespace SharpShuffleBag.Samples;

// ReSharper disable all
#pragma warning disable

public class SyntaxExamples
{
	public void Constructors()
	{
		// A simply empty bag.
		var shuffleBag0 = new ShuffleBag();

		// A bag with 3 items.
		var shuffleBag1 = new ShuffleBag<int> { 1, 2, 3 };

		// Creates an empty bag with a capacity of 3 (to which items can be added efficiently later).
		ShuffleBag<string> shuffleBag2 = new(capacity: 3);

		// The non-generic class is the same as 'ShuffleBag<int>'.
		ShuffleBag shuffleBag3 = new ShuffleBag();
	}

	public void AddingItems()
	{
		var bag = new ShuffleBag();

		bag.Add(0);
		bag.AddRange(1, 2, 3);
		bag.AddRange(Enumerable.Range(4, 3));
		bag.AddRange(new List<int> { 7, 8, 9 });

		// The size always reflects how many items were added up until now.
		int itemCount = bag.Size;
	}

	public void GettingRandomValues()
	{
		var bag = new ShuffleBag<int> { 1, 2, 3 };

		// Iterates once through all items in random order.
		foreach (int value in bag)
		{
		}

		// The same as above, but with a for-loop.
		for (int i = 0; i < bag.Size; i++)
		{
			int v = bag.Next();
		}

		// Iterating multiple times is possible.
		// The bag ensures that the last item from the previous iteration and
		// the first item of this iteration are never the same.
		foreach (int value in bag)
		{
		}

		// If you call Next without removing items (default),
		// the bag continues to return items in random order infinitely.
		for (int i = 0; i < 10; i++)
		{
			int v = bag.Next(markUsed: false);
		}

		// Start from the beginning (the internal counter is reset to zero as it was before calling Next).
		bag.Reset();

		while (bag.HasUnused)
		{
			// Using items is like taking them out of the bag until it is empty.
			int v = bag.Next(markUsed: true);
		}

		// However, be aware, that all originally added items are technically still in the bag.
		// This is what makes the implementation efficient, "removed" items are only marked as used.

		// This will "refill" the bag, but internally, only a counter is reset.
		bag.Reset();

		// This will actually remove all added items from the bag.
		bag.Clear();
	}

	public void SpecialUseCases()
	{
		int[] items = { 3, 5, 7 };
		var bag = new ShuffleBag(items);
		bag.RandomSource = new FixedSequenceSource(items);

		foreach (int i in bag)
		{
			// This always return 3, 5, 7.
		}

		// Or we could use one of the builtin sources to specify a custom seed.
		bag.RandomSource = new SystemRandomSource(seed: 42);
	}

	private class FixedSequenceSource : IRandomRangeSource
	{
		private readonly int[] items;

		public FixedSequenceSource(int[] items)
		{
			this.items = items;
		}

		public int Range(int minInclusive, int maxExclusive)
		{
			// Instead of returning a random value, we return the next consecutive items.
			// This can be used for testing scenarios in which a fixed sequence is more convenient.
			return items[minInclusive];
		}
	}

	public void ShuffleExtensions()
	{
		// As an addon, the Shuffle utility class can be used to shuffle any IList<T>.
		var list = new List<string> { "a", "b", "c" };
		Shuffle.FisherYates(list);
		Shuffle.FisherYates(list, new SystemRandomSource(seed: 42));
	}

	public void ReadmeExample()
	{
		ShuffleBag<string> cradle = new() { "Whiskers", "Mia", "Dabbles" };

		// Optionally, set a custom random source.
		cradle.RandomSource = new SystemRandomSource(seed: 42);

		// Retrieve 9 random cat names. Each cat is picked 3 times and no cat is picked twice in a row.
		for (int i = 0; i < 9; i++)
		{
			string randomCat = cradle.Next();
		}
	}
}
#pragma warning restore