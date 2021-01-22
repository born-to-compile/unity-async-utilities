using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BornToCompile.AsyncUtilities
{
	public class WaitForBackgroundThread
	{
		public ConfiguredTaskAwaitable.ConfiguredTaskAwaiter GetAwaiter()
		{
			return Task.Run(() => { }).ConfigureAwait(false).GetAwaiter();
		}
	}
}