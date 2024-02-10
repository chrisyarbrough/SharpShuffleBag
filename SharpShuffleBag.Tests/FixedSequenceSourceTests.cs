namespace SharpShuffleBag.Tests;

public class FixedSequenceSourceTests
{
	[Fact]
	public void SmokeTest()
	{
		var source = new FixedSequenceSource(3, 5, 7);
		source.Range(0, 3).Should().Be(3);
		source.Range(1, 3).Should().Be(5);
		source.Range(2, 3).Should().Be(7);
	}
}