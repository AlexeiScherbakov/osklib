using System;
using System.Runtime.InteropServices;

namespace Osklib.Interop
{
    internal static class NativeMethods
    {
		[DllImport( "user32.dll",EntryPoint = "FindWindow" )]
		internal static extern int FindWindow( string lpClassName, string lpWindowName );

		[DllImport( "user32.dll", EntryPoint = "SendMessage" )]
		internal static extern int SendMessage( int hWnd, uint Msg, int wParam, int lParam );

		[DllImport( "user32.dll",EntryPoint = "GetDesktopWindow", SetLastError = false )]
		internal static extern IntPtr GetDesktopWindow();

		[DllImport( "user32.dll", EntryPoint = "GetWindowLong" )]
		internal static extern int GetWindowLong( int hWnd, int nIndex );

		internal const int GWL_STYLE = -16;
		internal const int WM_SYSCOMMAND = 0x0112;
		internal const int SC_CLOSE = 0xF060;

		internal const int WS_DISABLED = 0x08000000;

		internal const int WS_VISIBLE = 0x10000000;

		internal static bool IsVisible(int style )
		{
			if (( style & WS_DISABLED )!=0)
			{
				return false;
			}
			if ( ( style & WS_VISIBLE ) != 0 )
			{
				return true;
			}
			return false;
		}

	}
}
