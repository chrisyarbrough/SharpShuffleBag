namespace SharpShuffleBag
{
	using System.Collections.Generic;

	public static class Shuffle
	{
		public static void FisherYates<T>(IList<T> list)
		{
			FisherYates(list, IRandomRangeSource.Default);
		}

		public static void FisherYates<T>(IList<T> list, IRandomRangeSource randomSource)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = randomSource.Range(0, n + 1);
				(list[k], list[n]) = (list[n], list[k]);
			}
		}
	}
}