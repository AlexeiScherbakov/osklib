using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

using Osklib.Interop;
using System.Runtime.InteropServices;
using System.Threading;

namespace Osklib
{
	public static class OnScreenKeyboard
	{

		private static readonly OnScreenKeyboardController _oskController;

		static OnScreenKeyboard()
		{
			var version = Environment.OSVersion.Version;
			switch ( version.Major )
			{
				case 6:
					switch ( version.Minor )
					{
						case 2:
							// Windows 10 (ok)
							_oskController = new Windows10OnScreenKeyboardController();
							break;
					}
					break;
				default:
					break;
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
