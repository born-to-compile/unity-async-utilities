using System;
using UnityEngine;

namespace BornToCompile.AsyncUtilities
{
	public static class Awaiters
	{
		public static WaitForUpdate NextFrame { get; } = new WaitForUpdate();

		public static WaitForFixedUpdate FixedUpdate { get; } = new WaitForFixedUpdate();

		public static WaitForEndOfFrame EndOfFrame { get; } = new WaitForEndOfFrame();

		public static WaitForSeconds Seconds(float seconds) => new WaitForSeconds(seconds);

		public static WaitForSecondsRealtime SecondsRealtime(float seconds) => new WaitForSecondsRealtime(seconds);

		public static WaitUntil Until(Func<bool> predicate) => new WaitUntil(predicate);

		public static WaitWhile While(Func<bool> predicate) => new WaitWhile(predicate);
	}
}