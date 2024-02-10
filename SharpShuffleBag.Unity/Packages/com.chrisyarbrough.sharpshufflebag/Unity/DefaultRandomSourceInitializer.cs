namespace SharpShuffleBag.Unity
{
	using UnityEngine;

	internal static class DefaultRandomSourceInitializer
	{
#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
#endif
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void Initialize()
		{
			if (IRandomRangeSource.Default.GetType() != typeof(UnityRandomSource))
			{
				IRandomRangeSource.Default = new UnityRandomSource();
			}
		}
	}
}