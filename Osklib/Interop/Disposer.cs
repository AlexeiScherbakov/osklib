using System;
using System.Threading;

namespace Osklib.Interop
{
	public static class Disposer
	{
		public static void Dispose<T>(ref T value)
			where T : class, IDisposable
		{
			var obj = Interlocked.Exchange(ref value, null);
			if (null != obj)
			{
				obj.Dispose();
			}
		}
	}
}
