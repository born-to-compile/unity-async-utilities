using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BornToCompile.AsyncUtilities.Internal
{
	internal class CoroutineWrapper<T>
	{
		private readonly SimpleCoroutineAwaiter<T> awaiter;
		private readonly Stack<IEnumerator> processStack;

		public CoroutineWrapper(IEnumerator coroutine, SimpleCoroutineAwaiter<T> awaiter)
		{
			processStack = new Stack<IEnumerator>();
			processStack.Push(coroutine);
			this.awaiter = awaiter;
		}

		public IEnumerator Run()
		{
			while (true)
			{
				var topWorker = processStack.Peek();

				bool isDone;

				try
				{
					isDone = !topWorker.MoveNext();
				}
				catch (Exception e)
				{
					// The IEnumerators we have in the process stack do not tell us the
					// actual names of the coroutine methods but it does tell us the objects
					// that the IEnumerators are associated with, so we can at least try
					// adding that to the exception output
					var objectTrace = GenerateObjectTrace(processStack);

					if (objectTrace.Any())
					{
						awaiter.Complete(default, new Exception(GenerateObjectTraceMessage(objectTrace), e));
					}
					else
					{
						awaiter.Complete(default, e);
					}

					yield break;
				}

				if (isDone)
				{
					processStack.Pop();

					if (processStack.Count == 0)
					{
						awaiter.Complete((T) topWorker.Current, null);
						yield break;
					}
				}

				// We could just yield return nested IEnumerator's here but we choose to do
				// our own handling here so that we can catch exceptions in nested coroutines
				// instead of just top level coroutine
				if (topWorker.Current is IEnumerator)
				{
					processStack.Push((IEnumerator) topWorker.Current);
				}
				else
				{
					// Return the current value to the unity engine so it can handle things like
					// WaitForSeconds, WaitToEndOfFrame, etc.
					yield return topWorker.Current;
				}
			}
		}
		
		private static string GenerateObjectTraceMessage(IEnumerable<Type> objTrace)
		{
			var result = new StringBuilder();

			foreach (var objType in objTrace)
			{
				if (result.Length != 0)
				{
					result.Append(" -> ");
				}

				result.Append(objType.ToString());
			}

			result.AppendLine();
			return "Unity Coroutine Object Trace: " + result;
		}

		private static List<Type> GenerateObjectTrace(IEnumerable<IEnumerator> enumerators)
		{
			var objTrace = new List<Type>();

			foreach (var enumerator in enumerators)
			{
				// NOTE: This only works with scripting engine 4.6
				// And could easily stop working with unity updates
				var field = enumerator.GetType().GetField("$this", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

				if (field == null)
				{
					continue;
				}

				var obj = field.GetValue(enumerator);

				if (obj == null)
				{
					continue;
				}

				var objType = obj.GetType();

				if (!objTrace.Any() || objType != objTrace.Last())
				{
					objTrace.Add(objType);
				}
			}

			objTrace.Reverse();
			return objTrace;
		}
	}
}