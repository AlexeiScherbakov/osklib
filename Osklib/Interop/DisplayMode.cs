namespace Osklib.Interop
{
	internal enum DisplayMode
	{
		NotSupported = 0,
		Floating = 2,
		Docked = 3,
	}

	internal static class DisplayModeExtension
	{
		public static OnScreenKeyboardDisplayMode ToPublicDisplayMode(this DisplayMode displayMode)
		{
			switch (displayMode)
			{
				case Interop.DisplayMode.NotSupported:
					return OnScreenKeyboardDisplayMode.NotSupported;
				case Interop.DisplayMode.Floating:
					return OnScreenKeyboardDisplayMode.Floating;
				case Interop.DisplayMode.Docked:
					return OnScreenKeyboardDisplayMode.Docked;
				default:
					return OnScreenKeyboardDisplayMode.None;
			}
		}
	}
}
