using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BornToCompile.AsyncUtilities.Internal
{
	internal static class InstructionWrappers
	{
		public static IEnumerator ReturnVoid(
			SimpleCoroutineAwaiter awaiter, object instruction)
		{
			// For simple instructions we assume that they don't throw exceptions
			yield return instruction;
			awaiter.Complete(null);
		}

		public static IEnumerator AssetBundleCreateRequest(
			SimpleCoroutineAwaiter<AssetBundle> awaiter, AssetBundleCreateRequest instruction)
		{
			yield return instruction;
			awaiter.Complete(instruction.assetBundle, null);
		}

		public static IEnumerator ReturnSelf<T>(
			SimpleCoroutineAwaiter<T> awaiter, T instruction)
		{
			yield return instruction;
			awaiter.Complete(instruction, null);
		}

		public static IEnumerator AssetBundleRequest(
			SimpleCoroutineAwaiter<UnityEngine.Object> awaiter, AssetBundleRequest instruction)
		{
			yield return instruction;
			awaiter.Complete(instruction.asset, null);
		}

		public static IEnumerator ResourceRequest(
			SimpleCoroutineAwaiter<UnityEngine.Object> awaiter, ResourceRequest instruction)
		{
			yield return instruction;
			awaiter.Complete(instruction.asset, null);
		}

		public static IEnumerator AddressableRequest<TObject>(
			SimpleCoroutineAwaiter<TObject> awaiter, AsyncOperationHandle<TObject> instruction)
		{
			yield return instruction;
			awaiter.Complete(instruction.Result, null);
		}
	}
}