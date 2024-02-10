namespace SharpShuffleBag.Tests;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit.Abstractions;

public sealed class ShuffleBagTests
{
	private readonly ITestOutputHelper testOutputHelper;

	public ShuffleBagTests(ITestOutputHelper testOutputHelper)
	{
		this.testOutputHelper = testOutputHelper;
	}

	[Fact]
	public void ShuffleBag_DefaultConstructor_CreatesEmptyBag()
	{
		var bag = new ShuffleBag<int>();
		bag.Size.Should().Be(0);
	}

	[Fact]
	public void ShuffleBag_CapacityConstructor_CreatesEmptyBag()
	{
		var bag = new ShuffleBag<int>(7);
		bag.Size.Should().Be(0);
	}

	[Fact]
	public void Add_SingleItem_IncrementsCount()
	{
		var bag = new ShuffleBag<int>();
		bag.Add(42);
		bag.Size.Should().Be(1);
	}

	[Fact]
	public void Add_MultipleItems_IncrementsCount()
	{
		var bag = new ShuffleBag<int> { 42, 7 };
		bag.Size.Should().Be(2);
	}

	[Fact]
	public void AddRange_MultipleItems_IncrementsCount()
	{
		var bag = new ShuffleBag<int>();
		bag.AddRange(new List<int> { 42, 7, 9 });
		bag.Size.Should().Be(3);
	}

	[Fact]
	public void Next_EmptyBag_ThrowsException()
	{
		var bag = new ShuffleBag<int>();
		bag.Invoking(b => b.Next()).Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void Next_BagWithSingleItem_ReturnsItem()
	{
		var bag = new ShuffleBag<int>();
		const int expected = 42;
		bag.Add(expected);
		bag.Next().Should().Be(expected);
	}

	[Fact]
	public void NextRemove_BagWithSingleItem_ReturnsItem()
	{
		var bag = new ShuffleBag<int>();
		const int expected = 42;
		bag.Add(expected);
		bag.Next(true).Should().Be(expected);
	}

	[Fact]
	public void NextRemove_BagWithMultipleItems_ReturnsItems()
	{
		var expected = Enumerable.Range(0, 3).ToImmutableArray();
		ShuffleBag<int> bag = new ShuffleBag<int>(expected);

		for (int i = 0; i < expected.Length; i++)
		{
			bag.Next(true);
		}

		bag.HasUnused.Should().Be(false);
	}

	[Fact]
	public void Next_BagWithSingleItemCalledMultipleTimes_ReturnsItem()
	{
		var bag = new ShuffleBag<int>();
		const int expected = 42;
		bag.Add(expected);

		int actual0 = bag.Next();
		int actual1 = bag.Next();

		actual0.Should().Be(expected);
		actual1.Should().Be(expected);
	}

	[Fact]
	public void Next_BagWithTwoItems_ReturnsAlternating()
	{
		var bag = new ShuffleBag<int> { 42, 7 };
		bag.RandomSource = new FixedSequenceSource(0, 1);

		int actual0 = bag.Next();
		int actual1 = bag.Next();
		int actual2 = bag.Next();

		actual0.Should().Be(42);
		actual1.Should().Be(7);
		actual2.Should().Be(42);
	}

	[Fact]
	public void Reset_BagWithTwoItems_ReturnsFirstAfterResetting()
	{
		var bag = new ShuffleBag<int> { 42, 7 };
		bag.RandomSource = new FixedSequenceSource(0, 1);

		int actual0 = bag.Next();
		bag.Reset();
		int actual1 = bag.Next();

		actual1.Should().Be(actual0);
	}

	[Fact]
	public void Clear_AfterAddingItems_SetsCountToZero()
	{
		var bag = new ShuffleBag<int>();
		bag.Add(1);
		bag.Clear();
		bag.Size.Should().Be(0);
		bag.HasUnused.Should().Be(false);
	}

	[Fact]
	public void Clear_AndCallingNext_Throws()
	{
		var bag = new ShuffleBag<int>();
		bag.Add(1);
		bag.Clear();
		bag.Invoking(b => b.Next()).Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void HasUniqueItems_WhenEmpty_ReportsFalse()
	{
		var bag = new ShuffleBag<int>();
		bag.HasUnused.Should().Be(false);
	}

	[Fact]
	public void HasUniqueItems_WithOneItem_ReportsTrue()
	{
		var bag = new ShuffleBag<int>(new List<int> { 42 });
		bag.HasUnused.Should().Be(true);
	}

	[Fact]
	public void Next_WithRemove_EmptiesBag()
	{
		var bag = new ShuffleBag<int>();
		bag.Add(1);
		bag.HasUnused.Should().Be(true);
		bag.Next(markUsed: true);
		bag.HasUnused.Should().Be(false);
	}

	[Fact]
	public void Next_WithRemove_EmptiesBag_02()
	{
		var bag = new ShuffleBag<int>();
		bag.Add(1);
		bag.Add(2);

		bag.HasUnused.Should().Be(true);
		bag.Next(markUsed: true);
		bag.HasUnused.Should().Be(true);

		bag.Next(markUsed: true);
		bag.HasUnused.Should().Be(false);
	}

	[Fact]
	public void ForeachIterationTest()
	{
		var set = new HashSet<int> { 1, 2, 3 };
		var bag = new ShuffleBag<int>(set);

		foreach (int i in bag)
		{
			testOutputHelper.WriteLine(i.ToString());
			set.Remove(i);
		}

		set.Should().BeEmpty();
	}

	[Fact]
	public void EnumeratorSupportsMultipleEnumeration()
	{
		var set = new HashSet<int> { 1, 2, 3 };
		var bag = new ShuffleBag<int>(set);

		using (var enumerator = bag.GetEnumerator())
		{
			Iterate(enumerator);
			enumerator.Reset();
			Iterate(enumerator);
		}

		set.Should().BeEmpty();
		return;

		void Iterate(ShuffleBag<int>.RandomEnumerator enumerator)
		{
			while (enumerator.MoveNext())
			{
				testOutputHelper.WriteLine(enumerator.Current.ToString());
				set.Remove(enumerator.Current);
			}
		}
	}

	[Fact]
	public void WhileIterationTest()
	{
		var set = new HashSet<int> { 1, 2, 3 };
		var bag = new ShuffleBag<int>(set);

		while (bag.HasUnused)
		{
			int i = bag.Next(markUsed: true);
			testOutputHelper.WriteLine(i.ToString());
			set.Remove(i);
		}

		set.Should().BeEmpty();

		// Used items should not actually be removed from the internal collection.
		bag.Size.Should().Be(3);
	}
}