namespace SharpShuffleBag.Unity.Editor
{
	using System.IO;
	using System.Linq;
	using UnityEditor;

	/// <summary>
	/// To edit the samples, import them into the Assets directory via the Package Manager window.
	/// Then, use this menu item to copy them pack to the Samples~ directory in the package.
	/// </summary>
	internal static class SampleDevelopment
	{
		private const string menuName = "Sharp Shuffle Bag/Copy Samples from Assets to Package";
		private static readonly DirectoryInfo importedSamplesDirectory = new("Assets/Samples/Sharp Shuffle Bag");

		[MenuItem(menuName)]
		public static void CopySamplesToPackage()
		{
			// Assets/Samples/Sharp Shuffle Bag/1.0.0/Example1
			// Assets/Samples/Sharp Shuffle Bag/1.0.0/Example2
			foreach (DirectoryInfo versionDirectory in importedSamplesDirectory.EnumerateDirectories().Take(1))
			{
				foreach (DirectoryInfo source in versionDirectory.EnumerateDirectories())
				{
					// Packages/com.xarbrough.sharp-shuffle-bag-unity/Samples~/Example1
					// Packages/com.xarbrough.sharp-shuffle-bag-unity/Samples~/Example2
					string dest = "Packages/com.chrisyarbrough.sharpshufflebag/Samples~/" + source.Name;
					CopyDirectory(source, dest);
				}
			}

			EditorUtility.DisplayDialog("Samples Copied", "The samples have been copied to the package.", "OK");
		}

		[MenuItem(menuName, isValidateFunction: true)]
		public static bool CopySamplesToPackage_Validate()
		{
			return importedSamplesDirectory.Exists;
		}

		private static void CopyDirectory(DirectoryInfo sourceDir, string destinationDir)
		{
			Directory.CreateDirectory(destinationDir);

			foreach (FileInfo file in sourceDir.GetFiles())
			{
				string targetFilePath = Path.Combine(destinationDir, file.Name);
				file.CopyTo(targetFilePath, overwrite: true);
			}

			foreach (DirectoryInfo subDir in sourceDir.GetDirectories())
			{
				string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
				CopyDirectory(subDir, newDestinationDir);
			}
		}
	}
}