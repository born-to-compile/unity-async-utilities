using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using BornToCompile.AsyncUtilities.Internal;

namespace BornToCompile.AsyncUtilities
{
	public class SimpleCoroutineAwaiter<T> : INotifyCompletion
	{
		public bool IsCompleted { get; private set; }
		
		private Exception exception;
		private Action continuation;
		private T result;

		public T GetResult()
		{
			Utilities.Assert(IsCompleted);

			if (exception != null)
			{
				ExceptionDispatchInfo.Capture(exception).Throw();
			}

			return result;
		}

		public void Complete(T result, Exception e)
		{
			Utilities.Assert(!IsCompleted);

			IsCompleted = true;
			exception = e;
			this.result = result;

			// Always trigger the continuation on the unity thread when awaiting on unity yield
			// instructions
			if (continuation != null)
			{
				Utilities.RunOnUnityScheduler(continuation);
			}
		}

		void INotifyCompletion.OnCompleted(Action continuation)
		{
			Utilities.Assert(this.continuation == null);
			Utilities.Assert(!IsCompleted);

			this.continuation = continuation;
		}
	}

	
	public class SimpleCoroutineAwaiter : INotifyCompletion
	{
		public bool IsCompleted => isDone;

		private bool isDone;
		private Exception exception;
		private Action continuation;

		public void GetResult()
		{
			Utilities.Assert(isDone);

			if (exception != null)
			{
				ExceptionDispatchInfo.Capture(exception).Throw();
			}
		}

		public void Complete(Exception e)
		{
			Utilities.Assert(!isDone);

			isDone = true;
			exception = e;

			// Always trigger the continuation on the unity thread when awaiting on unity yield
			// instructions
			if (continuation != null)
			{
				Utilities.RunOnUnityScheduler(continuation);
			}
		}

		void INotifyCompletion.OnCompleted(Action continuation)
		{
			Utilities.Assert(this.continuation == null);
			Utilities.Assert(!isDone);

			this.continuation = continuation;
		}
	}
}