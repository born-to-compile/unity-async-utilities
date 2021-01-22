using System.Collections;
using System.Collections.Generic;
using BornToCompile.AsyncUtilities.Internal;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

// We could just add a generic GetAwaiter to YieldInstruction and CustomYieldInstruction
// but instead we add specific methods to each derived class to allow for return values
// that make the most sense for the specific instruction type
namespace BornToCompile.AsyncUtilities
{
	public static class EnumeratorAwaitExtensions
	{
		public static SimpleCoroutineAwaiter GetAwaiter(this WaitForSeconds instruction)
		{
			return GetAwaiterReturnVoid(instruction);
		}

		public static SimpleCoroutineAwaiter GetAwaiter(this WaitForUpdate instruction)
		{
			return GetAwaiterReturnVoid(instruction);
		}

		public static SimpleCoroutineAwaiter GetAwaiter(this WaitForEndOfFrame instruction)
		{
			return GetAwaiterReturnVoid(instruction);
		}

		public static SimpleCoroutineAwaiter GetAwaiter(this WaitForFixedUpdate instruction)
		{
			return GetAwaiterReturnVoid(instruction);
		}

		public static SimpleCoroutineAwaiter GetAwaiter(this WaitForSecondsRealtime instruction)
		{
			return GetAwaiterReturnVoid(instruction);
		}

		public static SimpleCoroutineAwaiter GetAwaiter(this WaitUntil instruction)
		{
			return GetAwaiterReturnVoid(instruction);
		}

		public static SimpleCoroutineAwaiter GetAwaiter(this WaitWhile instruction)
		{
			return GetAwaiterReturnVoid(instruction);
		}

		public static SimpleCoroutineAwaiter<AsyncOperation> GetAwaiter(this AsyncOperation instruction)
		{
			return GetAwaiterReturnSelf(instruction);
		}

		public static SimpleCoroutineAwaiter<AsyncOperationHandle<IResourceLocator>> GetAwaiter(this AsyncOperationHandle<IResourceLocator> instruction)
		{
			return GetAwaiterReturnSelf(instruction);
		}

		public static SimpleCoroutineAwaiter<TObject> GetAwaiter<TObject>(this AsyncOperationHandle<TObject> instruction)
		{
			var awaiter = new SimpleCoroutineAwaiter<TObject>();
			Utilities.RunOnUnityScheduler(() => AsyncCoroutineRunner.Instance.StartCoroutine(
				InstructionWrappers.AddressableRequest(awaiter, instruction)));
			return awaiter;
		}

		public static SimpleCoroutineAwaiter<UnityEngine.Object> GetAwaiter(this ResourceRequest instruction)
		{
			var awaiter = new SimpleCoroutineAwaiter<UnityEngine.Object>();
			Utilities.RunOnUnityScheduler(() => AsyncCoroutineRunner.Instance.StartCoroutine(
				InstructionWrappers.ResourceRequest(awaiter, instruction)));
			return awaiter;
		}

		public static SimpleCoroutineAwaiter<UnityWebRequest> GetAwaiter(this UnityWebRequest instruction)
		{
			return GetAwaiterReturnSelf(instruction);
		}

		public static SimpleCoroutineAwaiter<AssetBundle> GetAwaiter(this AssetBundleCreateRequest instruction)
		{
			var awaiter = new SimpleCoroutineAwaiter<AssetBundle>();
			Utilities.RunOnUnityScheduler(() => AsyncCoroutineRunner.Instance.StartCoroutine(
				InstructionWrappers.AssetBundleCreateRequest(awaiter, instruction)));
			return awaiter;
		}

		public static SimpleCoroutineAwaiter<UnityEngine.Object> GetAwaiter(this AssetBundleRequest instruction)
		{
			var awaiter = new SimpleCoroutineAwaiter<UnityEngine.Object>();
			Utilities.RunOnUnityScheduler(() => AsyncCoroutineRunner.Instance.StartCoroutine(
				InstructionWrappers.AssetBundleRequest(awaiter, instruction)));
			return awaiter;
		}

		public static SimpleCoroutineAwaiter<T> GetAwaiter<T>(this IEnumerator<T> coroutine)
		{
			var awaiter = new SimpleCoroutineAwaiter<T>();
			Utilities.RunOnUnityScheduler(() => AsyncCoroutineRunner.Instance.StartCoroutine(
				new CoroutineWrapper<T>(coroutine, awaiter).Run()));
			return awaiter;
		}

		public static SimpleCoroutineAwaiter<object> GetAwaiter(this IEnumerator coroutine)
		{
			var awaiter = new SimpleCoroutineAwaiter<object>();
			Utilities.RunOnUnityScheduler(() => AsyncCoroutineRunner.Instance.StartCoroutine(
				new CoroutineWrapper<object>(coroutine, awaiter).Run()));
			return awaiter;
		}

		private static SimpleCoroutineAwaiter GetAwaiterReturnVoid(object instruction)
		{
			var awaiter = new SimpleCoroutineAwaiter();
			Utilities.RunOnUnityScheduler(() => AsyncCoroutineRunner.Instance.StartCoroutine(
				InstructionWrappers.ReturnVoid(awaiter, instruction)));
			return awaiter;
		}

		private static SimpleCoroutineAwaiter<T> GetAwaiterReturnSelf<T>(T instruction)
		{
			var awaiter = new SimpleCoroutineAwaiter<T>();
			Utilities.RunOnUnityScheduler(() => AsyncCoroutineRunner.Instance.StartCoroutine(
				InstructionWrappers.ReturnSelf(awaiter, instruction)));
			return awaiter;
		}
	}
}