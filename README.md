# Sharp Shuffle Bag

An efficient and convenient shuffle bag implementation in C# for .NET and Unity projects.

## Usage

```csharp
ShuffleBag<string> cradle = new() { "Whiskers", "Mia", "Dabbles" };

// Optionally, set a custom random source.
cradle.RandomSource = new SystemRandomSource(seed: 42);

// Retrieve 9 random cat names. Each cat is picked 3 times and no cat is picked twice in a row.
for (int i = 0; i < 9; i++)
{
    string randomCat = cradle.Next();
}
```

## Motivation

Random number generators, as often used in games, have characteristics which can feel unnatural
to humans. For example, a series of pseudo-random numbers like this:

```
[1, 2, 3, 4, 1, 1, 1, 1]
```

The shuffle bag algorithm attempts to make _random_ feel more natural:

- Fill a bag with numbers from 1 to 4
- Pick a random number from the bag
- Continue until the bag is empty
- Refill the bag

This will produce a non-repeating series like this:

```
[3, 4, 1, 2, 4, 3, 2, 1]
```

The _Sharp Shuffle Bag_ implements this algorithm without causing unnecessary GC allocations
and with consistent, fast performance. The randomization happens in-place while calling Next()
and therefore avoids the larger performance spike of reshuffling/refilling the bag when it's empty.

## Support

- .NET Standard 2.1
- Unity 2021.3 or newer

## Features

- Use the `ShuffleBag<T>` class to define a collection of items of type T.
- Use `shuffleBag.Next()` to get the next random item from the bag. The bag will never empty.
- Consecutive calls of `Next()` will never return the same item twice.
- Use `shuffleBag.Next(markUsed: true)` for a fixed-size bag.
- Efficient implementation: The underlying collection is not resized, instead,
  an index is used to perform the random selection.
- Set a custom `RandomSource` for fine-grained control (e.g. to make the bag deterministic or set a seed).
