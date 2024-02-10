using SharpShuffleBag;

ShuffleBag<string> cradle = new() { "Whiskers", "Simba", "Kitty", "Cleo", "Mia", "Dabbles" };

start:
Console.WriteLine($"Good evening! There are {cradle.Size} cats in the cradle.");
Console.WriteLine("Each cat leaves the cradle to go on an adventure. The order is: ");

while (cradle.HasUnused)
{
	string cat = cradle.Next(markUsed: true);
	Console.WriteLine($"- {cat}");
}

Console.WriteLine("All cats have left. Advance to morning? (y/n)");

if (Console.ReadKey(intercept: true).Key == ConsoleKey.Y)
{
	Console.WriteLine();
	cradle.Reset();
	goto start;
}