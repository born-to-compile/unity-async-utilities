using UnityEngine;

namespace BornToCompile.AsyncUtilities.Internal
{
	internal sealed class AsyncCoroutineRunner : MonoBehaviour
	{
		private static AsyncCoroutineRunner instance;

		public static AsyncCoroutineRunner Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new GameObject("AsyncCoroutineRunner").AddComponent<AsyncCoroutineRunner>();
				}

				return instance;
			}
		}

		void Awake()
		{
			// Don't show in scene hierarchy
			gameObject.hideFlags = HideFlags.HideAndDontSave;

			DontDestroyOnLoad(gameObject);
		}
	}
}