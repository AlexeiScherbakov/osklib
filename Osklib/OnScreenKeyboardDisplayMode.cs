namespace Osklib
{
	public enum OnScreenKeyboardDisplayMode
	{
		/// <summary>
		/// Not detected
		/// </summary>
		None = 0,
		/// <summary>
		/// Unsupported
		/// </summary>
		NotSupported = 1,
		/// <summary>
		/// Floating
		/// </summary>
		Floating = 2,
		/// <summary>
		/// Docked
		/// </summary>
		Docked = 3,

	}


	public static class OnScreenKeyboardDisplayModeExtension
	{
		public static bool IsVisibleDisplayMode(this OnScreenKeyboardDisplayMode mode)
		{
			return (mode == OnScreenKeyboardDisplayMode.Docked) || (mode == OnScreenKeyboardDisplayMode.Floating);
		}
	}
}
