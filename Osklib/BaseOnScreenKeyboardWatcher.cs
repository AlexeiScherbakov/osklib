using System;
using System.Runtime.InteropServices;
using Osklib.Interop;

namespace Osklib
{
	public abstract class BaseOnScreenKeyboardWatcher
		: IDisposable
	{
		private OnScreenKeyboardWatcherEvents _events;
		private bool? _lastState;
		private bool _closeOnTimeout;
		private DateTime? _openedSince;

		private IInputHostManagerBroker _inputHostManagerBroker;
		private Rect _location;
		private OnScreenKeyboardDisplayMode _displayMode;

		#region ctor & dtor
		protected BaseOnScreenKeyboardWatcher()
		{
			_events = OnScreenKeyboardWatcherEvents.State;
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

		protected abstract void Dispose(bool disposing);
		#endregion

		public OnScreenKeyboardWatcherEvents TrackedEvents
		{
			get { return _events; }
			set { _events = value; }
		}

		public OnScreenKeyboardDisplayMode DisplayMode
		{
			get { return _displayMode; }
		}

		public Rect Location
		{
			get { return _location; }
		}

		public bool? State
		{
			get { return _lastState; }
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
				Marshal.ReleaseComObject(_inputHostManagerBroker);
				_inputHostManagerBroker = null;
			}

			IImmersiveShellBroker shellBroker = null;
			try
			{
				shellBroker = (IImmersiveShellBroker)Activator.CreateInstance(ComTypes.ImmersiveShellBrokerType);
				_inputHostManagerBroker = shellBroker.GetInputHostManagerBroker();
				ret = _inputHostManagerBroker != null;
			}
			finally
			{
				if (!ReferenceEquals(shellBroker, null))
				{
					Marshal.ReleaseComObject(shellBroker);
				}
			}
			return ret;
		}

		private bool SetDisplayMode(DisplayMode displayMode)
		{
			OnScreenKeyboardDisplayMode setValue = OnScreenKeyboardDisplayMode.None;
			switch (displayMode)
			{
				case Interop.DisplayMode.NotSupported:
					setValue = OnScreenKeyboardDisplayMode.NotSupported;
					break;
				case Interop.DisplayMode.Floating:
					setValue = OnScreenKeyboardDisplayMode.Floating;
					break;
				case Interop.DisplayMode.Docked:
					setValue = OnScreenKeyboardDisplayMode.Docked;
					break;
			}
			if (_displayMode != setValue)
			{
				_displayMode = setValue;
				return true;
			}
			return false;
		}

		private OnScreenKeyboardWatcherRaiseEvents GetLocation()
		{ 
			if (ReferenceEquals(_inputHostManagerBroker, null))
			{
				if (!GetInputHostManagerBroker())
				{
					// cannot detect
					return OnScreenKeyboardWatcherRaiseEvents.None;
				}
			}
			DisplayMode displayMode;
			var ret = OnScreenKeyboardWatcherRaiseEvents.None;
			Rect location;
			_inputHostManagerBroker.GetIhmLocation(out location, out displayMode);
			if (!_location.Equals(location))
			{
				_location = location;
				ret |= OnScreenKeyboardWatcherRaiseEvents.LocationChanged;
			}
			if (SetDisplayMode(displayMode))
			{
				ret |= OnScreenKeyboardWatcherRaiseEvents.DisplayModeChanged;
			}
			return ret;
		}

		protected void CheckKeyboard()
		{
			// checking
			var raiseEvents = OnScreenKeyboardWatcherRaiseEvents.None;
			if (_events.HasFlag(OnScreenKeyboardWatcherEvents.Location))
			{
				raiseEvents|=GetLocation();
			}
			if (_events.HasFlag(OnScreenKeyboardWatcherEvents.State))
			{
				IntPtr wnd = NativeMethods.FindTextInputWindow();
				raiseEvents|=SetState(NativeMethods.IsValidHandle(wnd));
			}
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

		/// <summary>
		/// Sets field <paramref name="field"/> to <paramref name="value"/>
		/// </summary>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private static bool? SetTriStatePropertyValue(ref bool? field,bool value)
		{
			if (field.HasValue)
			{
				if (field.Value != value)
				{
					field = value;
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				field = value;
				return null;
			}
		}

		private OnScreenKeyboardWatcherRaiseEvents SetState(bool state)
		{
			switch(SetTriStatePropertyValue(ref _lastState, state))
			{
				case true:
					if (state)
					{
						_openedSince = DateTime.Now;
						//keyboard is shown
						return OnScreenKeyboardWatcherRaiseEvents.KeyboardOpened;
					}
					else
					{
						_openedSince = null;
						//keyboard is hidden
						return OnScreenKeyboardWatcherRaiseEvents.KeyboardClosed;
					}
					break;
				case false:
					if (!state)
					{
						_openedSince = null;
					}
					break;
			}
			return OnScreenKeyboardWatcherRaiseEvents.None;
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
