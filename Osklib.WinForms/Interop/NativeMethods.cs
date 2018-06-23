using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Osklib.WinForms.Interop
{
	internal static class NativeMethods
	{
		[DllImport("user32.dll", EntryPoint = "GetMessageExtraInfo")]
		private static extern uint GetMessageExtraInfo();


		public static bool IsTouchEvent()
		{
			uint extra = GetMessageExtraInfo();
			bool isTouchOrPen = ((extra & 0xFFFFFF00) == 0xFF515700);

			return isTouchOrPen;
			//bool isTouch = ((extra & 0x00000080) == 0x00000080);
			//else pen
		}
	}
}
