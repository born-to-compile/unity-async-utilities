using System;
using System.Threading;
using UnityEngine;

namespace BornToCompile.AsyncUtilities.Internal
{
	public class Utilities : MonoBehaviour
	{
		public static void Assert(bool condition)
		{
			if (!condition)
			{
				throw new Exception("BornToCompile.AsyncUtilities encountered an assertion.");
			}
		}
		
		public static void RunOnUnityScheduler(Action action)
		{
			if (SynchronizationContext.Current == SyncContextUtil.UnitySynchronizationContext)
			{
				action();
			}
			else
			{
				SyncContextUtil.UnitySynchronizationContext.Post(d => action(), null);
			}
		}
	}
}