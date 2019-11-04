using System;
using System.Runtime.InteropServices;
using Osklib.Interop;

namespace Osklib
{
	public abstract class BaseOnScreenKeyboardWatcher
		: IDisposable
	{
		private bool _closeOnTimeout;
		private DateTime? _openedSince;

		private ComPtr<IInputHostManagerBroker> _inputHostManagerBroker;
		private Rect _location;
		private OnScreenKeyboardDisplayMode _displayMode;

		#region ctor & dtor
		protected BaseOnScreenKeyboardWatcher()
		{
			_location = new Rect();
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

		protected virtual void Dispose(bool disposing)
		{
			Disposer.Dispose(ref _inputHostManagerBroker);
		}
		#endregion

		public OnScreenKeyboardDisplayMode DisplayMode
		{
			get { return _displayMode; }
		}

		public Rect Location
		{
			get { return _location; }
		}

		public DateTime? OpenedSince
		{
			get { return _openedSince; }
		}

		private bool GetInputHostManagerBroker()
		{
			bool ret = false;

			if (_inputHostManagerBroker != null)
			{
				Disposer.Dispose(ref _inputHostManagerBroker);
			}
			try
			{
				using (var shellBroker = ComPtr<IImmersiveShellBroker>.Create(ComTypes.ImmersiveShellBrokerType))
				{
					_inputHostManagerBroker = new ComPtr<IInputHostManagerBroker>(shellBroker.Instance.GetInputHostManagerBroker());
					ret = _inputHostManagerBroker != null;
				}
			}
			catch(Exception e)
			{

			}
			return ret;
		}

		private OnScreenKeyboardWatcherRaiseEvents SetDisplayMode(DisplayMode displayMode)
		{
			// обработка первой установки
			var setDisplayMode = displayMode.ToPublicDisplayMode();
			if (_displayMode == OnScreenKeyboardDisplayMode.None)
			{
				_displayMode = setDisplayMode;
				return OnScreenKeyboardWatcherRaiseEvents.None;
			}

			if (_displayMode == setDisplayMode)
			{
				// ничего не изменилось
				return OnScreenKeyboardWatcherRaiseEvents.None;
			}

			

			var ret = OnScreenKeyboardWatcherRaiseEvents.DisplayModeChanged;
			var oldStateVisible = (_displayMode == OnScreenKeyboardDisplayMode.Floating) || (_displayMode == OnScreenKeyboardDisplayMode.Docked);
			var newStateVisible = (setDisplayMode == OnScreenKeyboardDisplayMode.Floating) || (setDisplayMode == OnScreenKeyboardDisplayMode.Docked);

			if (oldStateVisible ^ newStateVisible)
			{
				// поменялась видимость
				if (newStateVisible)
				{
					ret |= OnScreenKeyboardWatcherRaiseEvents.KeyboardOpened;
				}
				else
				{
					ret |= OnScreenKeyboardWatcherRaiseEvents.KeyboardClosed;
				}
			}

			if (newStateVisible)
			{
				if (!_openedSince.HasValue)
				{
					_openedSince = DateTime.Now;
				}
			}
			else
			{
				_openedSince = null;
			}
			_displayMode = setDisplayMode;
			return ret;
		}


		protected void CheckKeyboard()
		{
			DisplayMode displayMode = Interop.DisplayMode.NotSupported;
			var raiseEvents = OnScreenKeyboardWatcherRaiseEvents.None;
			Rect location = default;
	
			int retry = 1;
			do
			{
				if (ReferenceEquals(_inputHostManagerBroker, null))
				{
					if (!GetInputHostManagerBroker())
					{
						// cannot detect
						return;
					}
				}
				try
				{
					_inputHostManagerBroker.Instance.GetIhmLocation(out location, out displayMode);
					retry = -1;
				}
				catch (Exception e)
				{
					retry--;
					Disposer.Dispose(ref _inputHostManagerBroker);
				}
			} while (retry >= 0);

			if (!_location.Equals(location))
			{
				_location = location;
				raiseEvents |= OnScreenKeyboardWatcherRaiseEvents.LocationChanged;
			}
			raiseEvents |= SetDisplayMode(displayMode);

			// raising events
			if (raiseEvents== OnScreenKeyboardWatcherRaiseEvents.None)
			{
				return;
			}
			else
			{
				OnParametersChanged();
			}

			if (raiseEvents.HasFlag(OnScreenKeyboardWatcherRaiseEvents.KeyboardOpened))
			{
				OnKeyboardOpened();
			}
			else if (raiseEvents.HasFlag(OnScreenKeyboardWatcherRaiseEvents.KeyboardClosed))
			{
				OnKeyboardClosed();
			}
		}

		protected abstract void OnParametersChanged();

		protected abstract void OnKeyboardOpened();
		protected abstract void OnKeyboardClosed();

		
	}


	[Flags]
	internal enum OnScreenKeyboardWatcherRaiseEvents
	{
		None,
		KeyboardOpened = 0b1,
		KeyboardClosed = 0b10,
		LocationChanged = 0b100,
		DisplayModeChanged = 0b1000
	}
}
