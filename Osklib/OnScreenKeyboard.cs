using System;

using Osklib.Interop;

namespace Osklib
{
	public static class OnScreenKeyboard
	{

		private static readonly OnScreenKeyboardController _oskController;


		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// Windows 10 Builds
		/// 14393 - 1607 (Anniversary Update)
		/// 15063 - 1703 (Creators Update)
		/// 16299 - 1709 (Fall Creators Update)
		/// </remarks>
		static OnScreenKeyboard()
		{
			Version version;
			if (NativeMethods.GetOsVersion(out version))
			{
				switch (version.Major)
				{
					case 10:
						// Windows 10 (ok)
						if (version.Build>= 18363)
						{
							_oskController = new ComOnScreenKeyboardController();
						}
						else if (version.Build > 16299)
						{
							_oskController = new NewWindows10OnScreenKeyboardController();
						}
						else
						{
							_oskController = new Windows10OnScreenKeyboardController();
						}
						
						break;
					default:
						break;
				}
			}
			
			if (null == _oskController)
			{
				_oskController = new UnsupportedOsOnScreenKeyboardController();
			}
		}

		

		public static void Show()
		{
			_oskController.Show();
		}

		public static bool IsOpened()
		{
			return _oskController.IsOpened();
		}

		public static bool Close()
		{
			return _oskController.Close();
		}
	}
}
