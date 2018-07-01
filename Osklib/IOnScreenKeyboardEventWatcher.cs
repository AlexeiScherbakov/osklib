using System;

namespace Osklib
{
	/// <summary>
	/// TODO this interface is added for WinForms compat (WinForms watcher has base class - Component)
	/// </summary>
	public interface IOnScreenKeyboardEventWatcher
	{
		bool? State { get; }
		DateTime? OpenedSince { get; }
		OnScreenKeyboardDisplayMode DisplayMode { get; }
		OnScreenKeyboardWatcherEvents TrackedEvents { get; }
		Rect Location { get; }

		event EventHandler ParametersChanged;
		event EventHandler KeyboardOpened;
		event EventHandler KeyboardClosed;
	}
}
