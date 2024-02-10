using System.Collections;
using UnityEngine;

namespace SharpShuffleBag.Unity.Samples
{
	internal sealed class Example : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] gameObjects;

		private ShuffleBag<GameObject> shuffleBag;

		private IEnumerator Start()
		{
			shuffleBag = new ShuffleBag<GameObject>(gameObjects);
			foreach (GameObject go in gameObjects)
				go.SetActive(false);

			while (Application.isPlaying)
			{
				GameObject go = shuffleBag.Next();
				go.SetActive(!go.activeSelf);
				yield return new WaitForSeconds(0.5f);
			}
		}
	}
}