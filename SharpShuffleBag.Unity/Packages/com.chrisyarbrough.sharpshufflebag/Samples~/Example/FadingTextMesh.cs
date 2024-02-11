namespace SharpShuffleBag.Unity.Samples
{
	using System.Collections;
	using UnityEngine;

	internal sealed class FadingTextMesh : MonoBehaviour
	{
		public string Text
		{
			get => text.text;
			set => text.text = value;
		}

		[SerializeField]
		private TextMesh text;

		[SerializeField]
		private float duration = 1f;

		private Color originalColor;

		private void Awake()
		{
			originalColor = text.color;
			text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
		}

		public void StartFade()
		{
			text.color = originalColor;
			StartCoroutine(DoFade());
		}

		private IEnumerator DoFade()
		{
			Color color = originalColor;

			for (float t = 0f; t < duration; t += Time.deltaTime)
			{
				text.color = new Color(color.r, color.g, color.b, 1f - t / duration);
				yield return null;
			}

			text.color = new Color(color.r, color.g, color.b, 0);
		}
	}
}