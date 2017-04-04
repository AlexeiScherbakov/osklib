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
							break;
					}
					break;
				default:
					break;
			}
		}

		private static void StartTabTip()
		{
			var p = Process.Start( @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe" );
			int handle = 0;
			while( (handle = NativeMethods.FindWindow( "IPTIP_Main_Window", "" ))<=0)
			{
				Thread.Sleep( 100 );
			}
		}

		private static void ShowByCom()
		{
			var type = Type.GetTypeFromCLSID( Guid.Parse( "4ce576fa-83dc-4F88-951c-9d0782b4e376" ) );
			var instance = (ITipInvocation) Activator.CreateInstance( type );
			instance.Toggle( NativeMethods.GetDesktopWindow() );
			Marshal.ReleaseComObject( instance );
		}

		public static void Show()
		{
			int handle = NativeMethods.FindWindow( "IPTIP_Main_Window", "" );
			if ( handle > 0 )
			{
				var style = NativeMethods.GetWindowLong( handle, NativeMethods.GWL_STYLE );
				//Console.WriteLine( "Style {0:X8}", style );
				if (! NativeMethods.IsVisible( style ) )
				{
					ShowByCom();
				}
			}
			else
			{
				StartTabTip();
				// on some devices starting TabTip don't show keyboard, on some does  ¯\_(ツ)_/¯
				if ( !IsOpened() )
				{
					ShowByCom();
				}
			}		
		}

		public static bool IsOpened()
		{
			int handle = NativeMethods.FindWindow( "IPTIP_Main_Window", "" );
			if ( handle <= 0 )
			{
				return false;
			}
			var style = NativeMethods.GetWindowLong( handle, NativeMethods.GWL_STYLE );
			//Console.WriteLine( "Style {0:X8}", style );
			return NativeMethods.IsVisible( style );
		}

		public static bool Close()
		{
			// find it
			int handle = NativeMethods.FindWindow( "IPTIP_Main_Window", "" );
			bool active = handle > 0;
			if ( active )
			{
				// don't check style - just close
				NativeMethods.SendMessage( handle, NativeMethods.WM_SYSCOMMAND, NativeMethods.SC_CLOSE, 0 );
			}
			return active;
		}
	}
}
