using System;
using System.Runtime.InteropServices;

namespace Osklib.Interop
{
    internal static class NativeMethods
    {
		#region [user32]
		[DllImport( "user32.dll",EntryPoint = "FindWindow" )]
		internal static extern IntPtr FindWindow( string lpClassName, string lpWindowName );

		[DllImport("user32.dll", EntryPoint = "FindWindowEx")]
		internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


		[DllImport( "user32.dll", EntryPoint = "SendMessage" )]
		internal static extern int SendMessage( IntPtr hWnd, uint Msg, int wParam, int lParam );

		[DllImport( "user32.dll",EntryPoint = "GetDesktopWindow", SetLastError = false )]
		internal static extern IntPtr GetDesktopWindow();

		[DllImport( "user32.dll", EntryPoint = "GetWindowLong" )]
		internal static extern int GetWindowLong( IntPtr hWnd, int nIndex );
		#endregion

		#region [dwmapi]
		[DllImport("dwmapi.dll", EntryPoint = "DwmGetWindowAttribute")]
		internal static extern int DwmGetWindowAttribute(IntPtr IntPtr, int dwAttribute,out int pvAttribute,uint cbAttribute);
		#endregion

		internal const int GWL_STYLE = -16;
		internal const int WM_SYSCOMMAND = 0x0112;
		internal const int SC_CLOSE = 0xF060;

		internal const int WS_DISABLED = 0x08000000;

		internal const int WS_VISIBLE = 0x10000000;

		internal const int DWMWA_CLOAKED = 14;

		internal static bool IsValidHandle(IntPtr handle)
		{
			// if will be eliminated by jit
			if (IntPtr.Size == 4)
			{
				return handle.ToInt32() > 0;
			}
			else
			{
				return handle.ToInt64() > 0;
			}
		}


		internal static bool IsTabTipProcessPresent()
		{
			IntPtr handle = NativeMethods.FindWindow("IPTIP_Main_Window", "");
			return NativeMethods.IsValidHandle(handle);
		}

		/// <summary>
		/// Check window for visibility
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		internal static bool? IsWindowVisibleByHandle(IntPtr handle )
		{
			var style = GetWindowLong(handle, GWL_STYLE);
			//Console.WriteLine( "Style {0:X8}", style );

			// if is disabled - not visible
			if (( style & WS_DISABLED )!=0)
			{
				return false;
			}
			// if has visible style - visible :)
			if ( ( style & WS_VISIBLE ) != 0 )
			{
				return true;
			}
			// DWM Window can be cloaked
			// see https://social.msdn.microsoft.com/Forums/vstudio/en-US/f8341376-6015-4796-8273-31e0be91da62/difference-between-actually-visible-and-not-visiblewhich-are-there-but-we-cant-see-windows-of?forum=vcgeneral
			int cloaked;
			if (DwmGetWindowAttribute(handle, DWMWA_CLOAKED, out cloaked, 4) == 0)
			{
				if (cloaked != 0)
				{
					return false;
				}
			}
			// undefined
			return null;
		}

		internal static bool IsWin10OnScreenKeyboardVisible()
		{
			IntPtr handle = NativeMethods.FindWindow("IPTIP_Main_Window", "");
			if (!IsValidHandle(handle))
			{
				return false;
			}

			var isVisible = NativeMethods.IsWindowVisibleByHandle(handle);
			if (isVisible.HasValue)
			{
				return isVisible.Value;
			}

			// hard way
			// enumerating Windows Store Container windows and try to find Window with caption 'Microsoft Text Input Application'
			IntPtr lastProbed = IntPtr.Zero;
			do
			{
				lastProbed = FindWindowEx(IntPtr.Zero, lastProbed, ApplicationFrameHostClassName, null);
				if (IsValidHandle(lastProbed))
				{
					var textInput = FindWindowEx(lastProbed, IntPtr.Zero, CoreWindowClassName, TextInputApplicationCaption);
					return IsValidHandle(textInput);
				}
			} while (IsValidHandle(lastProbed));

			return false;
		}

		private const string ApplicationFrameHostClassName = "ApplicationFrameWindow";

		private const string CoreWindowClassName= "Windows.UI.Core.CoreWindow";
		private const string TextInputApplicationCaption = "Microsoft Text Input Application";

	}
}
