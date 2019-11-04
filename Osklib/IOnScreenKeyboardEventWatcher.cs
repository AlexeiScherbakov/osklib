using System;

namespace Osklib
{
	/// <summary>
	/// TODO this interface is added for WinForms compat (WinForms watcher has base class - Component)
	/// </summary>
	public interface IOnScreenKeyboardEventWatcher
	{
		DateTime? OpenedSince { get; }
		OnScreenKeyboardDisplayMode DisplayMode { get; }
		Rect Location { get; }

		event EventHandler ParametersChanged;
		event EventHandler KeyboardOpened;
		event EventHandler KeyboardClosed;
	}


	public static class OnScreenKeyboardEventWatcherExtension
	{
		public static bool IsOpened<TWatcher>(this TWatcher watcher)
			where TWatcher:IOnScreenKeyboardEventWatcher
		{
			return watcher.DisplayMode.IsVisibleDisplayMode();
		}
	}
}
