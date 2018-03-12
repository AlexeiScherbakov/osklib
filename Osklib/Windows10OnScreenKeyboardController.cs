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

	internal sealed class Windows10OnScreenKeyboardController
		: OnScreenKeyboardController
	{
		public override bool Close()
		{
			// find it
			IntPtr handle = NativeMethods.FindWindow("IPTIP_Main_Window", "");
			bool active = NativeMethods.IsValidHandle(handle);
			if (active)
			{
				// don't check style - just close
				NativeMethods.SendMessage(handle, NativeMethods.WM_SYSCOMMAND, NativeMethods.SC_CLOSE, 0);
			}
			return active;
		}

		public override bool IsOpened()
		{
			return NativeMethods.IsTabTipProcessPresent() && NativeMethods.IsWin10OnScreenKeyboardVisible();
		}

		private static void StartTabTip()
		{
			var p = Process.Start(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
			IntPtr handle;
			while (!NativeMethods.IsValidHandle(handle = NativeMethods.FindWindow("IPTIP_Main_Window", "")))
			{
				Thread.Sleep(100);
			}
		}

		private static void ShowByCom()
		{
			var type = Type.GetTypeFromCLSID(Guid.Parse("4ce576fa-83dc-4F88-951c-9d0782b4e376"));
			var instance = (ITipInvocation)Activator.CreateInstance(type);
			instance.Toggle(NativeMethods.GetDesktopWindow());
			Marshal.ReleaseComObject(instance);
		}

		public override void Show()
		{
			// Detecting TabTip process presence
			if (NativeMethods.IsTabTipProcessPresent())
			{
				//Console.WriteLine( "Style {0:X8}", style );
				if (!NativeMethods.IsWin10OnScreenKeyboardVisible())
				{
					ShowByCom();
				}
			}
			else
			{
				StartTabTip();
				// on some devices starting TabTip don't show keyboard, on some does  ¯\_(ツ)_/¯
				if (!IsOpened())
				{
					ShowByCom();
				}
			}
		}
	}
}
