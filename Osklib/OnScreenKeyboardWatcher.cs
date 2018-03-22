using Osklib.Interop;
using System;
using System.Threading;

namespace Osklib
{
	public sealed class OnScreenKeyboardWatcher
		: IDisposable
	{
		private bool? _lastState;

		private Thread _watcherThread;
		private ManualResetEventSlim _endEvent;

		#region ctor & dtor
		public OnScreenKeyboardWatcher()
		{
			_endEvent = new ManualResetEventSlim(false);

			_watcherThread = new Thread(WatcherThreadFunc);
			// below normal priority background thread
			_watcherThread.Priority = ThreadPriority.BelowNormal;
			_watcherThread.IsBackground = true;
			_watcherThread.Start();
		}

		~OnScreenKeyboardWatcher()
		{
			Terminate();
		}

		public void Dispose()
		{
			Terminate();
			GC.SuppressFinalize(this);
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

		private void SetState(bool state)
		{
			if (_lastState.HasValue)
			{
				if (_lastState.Value != state)
				{
					_lastState = state;

					if (state)
					{
						//keyboard is shown
						var evnt = KeyboardOpened;
						if (evnt != null)
						{
							evnt(this, EventArgs.Empty);
						}
					}
					else
					{
						//keyboard is hidden
						var evnt = KeyboardClosed;
						if (evnt != null)
						{
							evnt(this, EventArgs.Empty);
						}
					}
				}
			}
			else
			{
				//just set
				_lastState = state;
			}
		}


		private void WatcherThreadFunc()
		{
			try
			{
				do
				{
					IntPtr wnd = NativeMethods.FindTextInputWindow();
					SetState(NativeMethods.IsValidHandle(wnd));
				} while (!_endEvent.Wait(100));
			}
			catch (ThreadAbortException tae)
			{
				Thread.ResetAbort();
			}
		}


		public event EventHandler KeyboardOpened;

		public event EventHandler KeyboardClosed;
	}
}
