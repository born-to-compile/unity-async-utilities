using System.Threading;
using UnityEngine;

namespace BornToCompile.AsyncUtilities.Internal
{
	internal static class SyncContextUtil
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Install()
		{
			UnitySynchronizationContext = SynchronizationContext.Current;
			UnityThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		public static int UnityThreadId { get; private set; }

		public static SynchronizationContext UnitySynchronizationContext { get; private set; }
	}
}