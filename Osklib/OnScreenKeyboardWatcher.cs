using System;
using System.Threading;

using Osklib.Interop;

namespace Osklib
{
	public sealed class OnScreenKeyboardWatcher
		: BaseEventOnScreenKeyboardWatcher
	{
		private Thread _watcherThread;
		private ManualResetEventSlim _endEvent;

		#region ctor & dtor
		public OnScreenKeyboardWatcher()
			:this(ThreadPriority.BelowNormal)
		{
		}

		public OnScreenKeyboardWatcher(ThreadPriority threadPriority)
		{
			CheckKeyboard();
			_endEvent = new ManualResetEventSlim(false);
			_watcherThread = new Thread(WatcherThreadFunc);
			// below normal priority background thread
			_watcherThread.Priority = threadPriority;
			_watcherThread.IsBackground = true;
			_watcherThread.Start();
		}

		protected override void Dispose(bool disposing)
		{
			Terminate();
		}
		#endregion

		private void Terminate()
		{
			_endEvent.Set();
			if (!_watcherThread.Join(500))
			{
				_watcherThread.Abort();
			}
		}

		private void WatcherThreadFunc()
		{
			try
			{
				do
				{
					CheckKeyboard();
				} while (!_endEvent.Wait(100));
			}
			catch (ThreadAbortException tae)
			{
				Thread.ResetAbort();
			}
		}
	}

}
