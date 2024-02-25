// ReSharper disable UnusedMember.Global

namespace SharpShuffleBag
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;

	/// <summary>
	/// <para>
	/// Returns a sequence of random non-repeating items from a set.
	/// </para>
	/// If the set contains only unique items, the bag ensures
	/// that two consecutive calls to <see cref="Next" /> return distinct values
	/// and that the entire set is used before a new set is started.
	/// </summary>
	/// <example><p>
	/// This sample shows basic creation and usage of the ShuffleBag. The bag
	/// can be iterates with both for- and foreach-loops.</p>
	/// <code><![CDATA[
	/// var shuffleBag = new ShuffleBag<int> { 1, 2, 3 };
	/// for (int i = 0; i < shuffleBag.Size; i++)
	/// {
	/// 	int randomValue = shuffleBag.Next();
	/// }
	///
	/// foreach (int randomValue in shuffleBag)
	/// {
	///
	/// }
	/// ]]></code>
	/// </example>
	[DebuggerDisplay("Size = {Size} Cursor = {cursor}")]
	public class ShuffleBag<T> : IEnumerable<T>
	{
		/// <summary>
		/// The items within the bag. Distinctness is not guaranteed.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		private readonly List<T> items;

		/// <summary>
		/// Indicates up to which index items have been used within the current set.
		/// </summary>
		private int cursor;

		/// <summary>
		/// Constructs an empty shuffle bag.
		/// </summary>
		public ShuffleBag()
		{
			items = new List<T>();
		}

		/// <summary>
		/// Constructs an empty shuffle bag with the provided capacity.
		/// </summary>
		/// <param name="capacity">The maximum number of items the bag should hold.</param>
		public ShuffleBag(int capacity)
		{
			items = new List<T>(capacity);
		}

		/// <summary>
		/// Constructs a shuffle bag filled with items.
		/// </summary>
		/// <param name="items">
		/// The items to be shuffled.<para>
		/// To ensure non-repeating values returned from <see cref="Next" />,
		/// items must contain at least two items and only distinct values.
		/// </para>
		/// </param>
		public ShuffleBag(IEnumerable<T> items)
		{
			this.items = new List<T>(items);
		}

		/// <summary>
		/// Sets the source for random values chosen by the bag.
		/// </summary>
		public IRandomRangeSource RandomSource
		{
			get => randomSource ?? IRandomRangeSource.Default;
			set => randomSource = value ?? throw new ArgumentNullException(nameof(value));
		}

		private IRandomRangeSource randomSource;

		/// <summary>
		/// The number of items added to the bag regardless whether items have been picked or not.
		/// Imagine this as the capacity of how many items the bag can hold in each set.
		/// </summary>
		public int Size => items.Count;

		/// <summary>
		/// Returns true while it is valid to call <see cref="Next(bool)" />.
		/// </summary>
		public bool HasUnused => cursor < items.Count;

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(T item) => items.Add(item);

		public void AddRange(IEnumerable<T> items) => this.items.AddRange(items);

		public void AddRange(params T[] items) => this.items.AddRange(items);

		/// <summary>
		/// Returns the next random value from the bag. If <paramref name="markUsed" /> is true,
		/// callers have to check <see cref="HasUnused" /> or else this method throws."/>
		/// <para>
		/// It is ensured that each item in the set is returned once
		/// before a new set is started. There are no repetitions within a set
		/// and two consecutive calls (e.g. the last item of the previous set
		/// and the first of the current set), will never return the same value, if
		/// the set contains at least two items and only distinct values.
		/// </para>
		/// </summary>
		/// <exception cref="System.InvalidOperationException">If <see cref="Size" /> is zero.</exception>
		public T Next(bool markUsed = false)
		{
			int count = items.Count;

			if (count == 0)
			{
				throw new InvalidOperationException(
					$"Cannot call {nameof(Next)}() on an empty bag. Add items before requesting values.");
			}

			// When starting a new set, ensure that we do not pick the last
			// item from the previous set to avoid a possible collision.
			int max = cursor == 0 ? count - 1 : count;

			int randomIndex = RandomSource.Range(cursor, max);
			RandomSource.Verify(randomIndex, cursor, max);

			// Swap the random pick with the cursor position,
			// so that items below the cursor are not picked again within this set.
			T currentItem = items[randomIndex];
			items[randomIndex] = items[cursor];
			items[cursor] = currentItem;

			if (markUsed)
				cursor++;
			else
				cursor = (cursor + 1) % count;

			return currentItem;
		}

		public void Reset()
		{
			cursor = 0;
		}

		public void Clear() => items.Clear();

		/// <summary>
		/// Iterates the current set in a random order.
		/// </summary>
		public RandomEnumerator GetEnumerator() => new RandomEnumerator(this);

		/// <summary>
		/// Iterates the set of items within <see cref="ShuffleBag{T}" /> in a random order.
		/// </summary>
		/// <remarks>
		/// The manual implementation of the enumerator as a struct was chosen
		/// to avoid garbage allocation during calls to IEnumerable{T}.GetEnumerator()
		/// when using e.g. a foreach loop to iterate over the shuffle bag.
		/// </remarks>
		public struct RandomEnumerator : IEnumerator<T>
		{
			public T Current { get; private set; }

			object IEnumerator.Current => Current;

			private readonly ShuffleBag<T> bag;
			private int index;

			internal RandomEnumerator(ShuffleBag<T> bag)
			{
				this.bag = bag;
				index = 0;
				Current = default;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				int count = bag.Size;

				if (index < count)
				{
					Current = bag.Next();
					index++;
					return true;
				}

				index = count;
				Current = default;
				return false;
			}

			public void Reset()
			{
				index = 0;
				Current = default;
			}
		}
	}
}