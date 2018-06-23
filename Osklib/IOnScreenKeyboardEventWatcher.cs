using System;

namespace Osklib
{
	public interface IOnScreenKeyboardEventWatcher
	{
		bool? State { get; }
		DateTime? OpenedSince { get; }

		event EventHandler KeyboardOpened;
		event EventHandler KeyboardClosed;
	}
}
