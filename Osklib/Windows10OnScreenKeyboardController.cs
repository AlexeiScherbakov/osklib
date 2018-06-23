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
		private static readonly Type TipInvocationType = null;

		static Windows10OnScreenKeyboardController()
		{
			TipInvocationType=Type.GetTypeFromCLSID(Guid.Parse("4ce576fa-83dc-4F88-951c-9d0782b4e376"));
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
			var p = Process.Start(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
			IntPtr handle;
			while (!NativeMethods.IsValidHandle(handle = NativeMethods.FindWindow("IPTIP_Main_Window", "")))
			{
				Thread.Sleep(100);
			}
		}

		private static void ShowByCom()
		{
			ITipInvocation instance = null;
			try
			{
				instance = (ITipInvocation)Activator.CreateInstance(TipInvocationType);
				var window = NativeMethods.GetDesktopWindow();
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
