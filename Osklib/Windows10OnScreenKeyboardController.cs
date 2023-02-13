using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using Osklib.Interop;

namespace Osklib
{

	internal sealed class Windows10OnScreenKeyboardController
		: OnScreenKeyboardController
	{
		static Windows10OnScreenKeyboardController()
		{
			
		}

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
			return NativeMethods.IsWin10OnScreenKeyboardVisible();
		}

		private static void StartTabTip()
		{
			ProcessStartInfo psi = new ProcessStartInfo(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe")
			{
				UseShellExecute = true
			};

			var p = Process.Start(psi);
			IntPtr handle;
			while (!NativeMethods.IsValidHandle(handle = NativeMethods.FindWindow("IPTIP_Main_Window", "")))
			{
				Thread.Sleep(100);
			}
		}

		private static void ToggleByCom()
		{
			ITipInvocation instance = null;
			try
			{
				instance = (ITipInvocation)Activator.CreateInstance(ComTypes.TipInvocationType);
				instance.Toggle(NativeMethods.GetDesktopWindow());
			}
			finally
			{
				if (!ReferenceEquals(instance, null))
				{
					Marshal.ReleaseComObject(instance);
				}
			}
		}

		public override void Show()
		{
			// Detecting TabTip process presence
			if (NativeMethods.IsTabTipProcessPresent())
			{
				//Console.WriteLine( "Style {0:X8}", style );
				if (!NativeMethods.IsWin10OnScreenKeyboardVisible())
				{
					ToggleByCom();
				}
			}
			else
			{
				StartTabTip();
				// on some devices starting TabTip don't show keyboard, on some does  ¯\_(ツ)_/¯
				Thread.Sleep(1000);
				if (!IsOpened())
				{
					ToggleByCom();
				}
			}
		}
	}
}
