using System;

namespace Osklib
{
	[Flags]
	public enum OnScreenKeyboardWatcherEvents
	{
		None,
		/// <summary>
		/// Watch for state
		/// </summary>
		State=0b01,
		/// <summary>
		/// Watch for position and mode
		/// </summary>
		Location=0b10
	}
}
