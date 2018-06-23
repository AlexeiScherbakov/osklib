using System;

using Osklib.Interop;

namespace Osklib
{
	public abstract class BaseOnScreenKeyboardWatcher
		: IDisposable
	{
		private bool? _lastState;
		private bool _closeOnTimeout;
		private DateTime? _openedSince;


		protected BaseOnScreenKeyboardWatcher()
		{
		}

		~BaseOnScreenKeyboardWatcher()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected abstract void Dispose(bool disposing);


		public bool? State
		{
			get { return _lastState; }
		}

		public DateTime? OpenedSince
		{
			get { return _openedSince; }
		}


		protected void CheckKeyboard()
		{
			IntPtr wnd = NativeMethods.FindTextInputWindow();
			SetState(NativeMethods.IsValidHandle(wnd));
		}

		protected void SetState(bool state)
		{
			if (_lastState.HasValue)
			{
				if (_lastState.Value != state)
				{
					_lastState = state;

					if (state)
					{
						_openedSince = DateTime.Now;
						//keyboard is shown
						OnKeyboardOpened();
					}
					else
					{
						_openedSince = null;
						//keyboard is hidden
						OnKeyboardClosed();
					}
				}
				else
				{
					if (!state)
					{
						_openedSince = null;
					}
				}
			}
			else
			{
				//just set (prevent fire event on watcher start)
				_lastState = state;
			}
		}

		protected abstract void OnKeyboardOpened();
		protected abstract void OnKeyboardClosed();
	}
}
