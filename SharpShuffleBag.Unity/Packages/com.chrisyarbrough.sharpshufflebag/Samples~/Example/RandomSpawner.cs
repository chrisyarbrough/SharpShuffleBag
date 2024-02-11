namespace SharpShuffleBag.Unity.Samples
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	internal sealed class RandomSpawner : MonoBehaviour
	{
		[SerializeField]
		private FadingTextMesh template;

		[SerializeField]
		private int count = 12;

		[SerializeField]
		private float ellipseWidth = 2f;

		[SerializeField]
		private float ellipseHeight = 2f;

		[SerializeField]
		private float spawnInterval = 0.5f;

		private IEnumerator Start()
		{
			Application.targetFrameRate = 60;

			ShuffleBag<FadingTextMesh> shuffleBag = new(SpawnTexts());

			int i = 0;
			while (Application.isPlaying)
			{
				yield return new WaitForSeconds(spawnInterval);

				FadingTextMesh text = shuffleBag.Next();
				text.StartFade();

				if (i++ % shuffleBag.Size == 0)
					Debug.Log("----");
				Debug.Log(text.Text);
			}
		}

		private IEnumerable<FadingTextMesh> SpawnTexts()
		{
			for (int i = 0; i < count; i++)
			{
				float angle = i * (Mathf.PI * 2) / count;
				Vector3 position = new Vector3(
					Mathf.Cos(angle) * ellipseWidth,
					Mathf.Sin(angle) * ellipseHeight,
					0);

				FadingTextMesh instance = Instantiate(template, position, Quaternion.identity);
				instance.Text += i.ToString();
				instance.gameObject.name = "Text-" + i;

				yield return instance;
			}

			template.gameObject.SetActive(false);
		}
	}
}