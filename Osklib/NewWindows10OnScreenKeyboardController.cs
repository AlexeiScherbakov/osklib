using System;
using System.Diagnostics;
using System.Threading;

using Osklib.Interop;

namespace Osklib
{
	internal sealed class NewWindows10OnScreenKeyboardController
		: OnScreenKeyboardController
	{
		private ComPtr<IInputHostManagerBroker> _inputHostManagerBroker;


		public NewWindows10OnScreenKeyboardController()
		{
			CreateInputHostManagerBroker();
		}

		private void CreateInputHostManagerBroker()
		{
			using (var shellBroker = ComPtr<IImmersiveShellBroker>.Create(ComTypes.ImmersiveShellBrokerType))
			{
				Disposer.Dispose(ref _inputHostManagerBroker);
				_inputHostManagerBroker = new ComPtr<IInputHostManagerBroker>(shellBroker.Instance.GetInputHostManagerBroker());
			}
		}

		public override bool Close()
		{
			// find it
			IntPtr handle = NativeMethods.FindWindow("IPTIP_Main_Window", "");
			if (NativeMethods.IsValidHandle(handle))
			{
				// don't check style - just close
				NativeMethods.SendMessage(handle, NativeMethods.WM_SYSCOMMAND, NativeMethods.SC_CLOSE, 0);
				return true;
			}
			return false;
		}

		public override bool IsOpened()
		{
			IntPtr handle = NativeMethods.FindWindow("IPTIP_Main_Window", "");
			if (!NativeMethods.IsValidHandle(handle))
			{
				// COM Server is inactive
				return false;
			}
			bool error = false;
			DisplayMode displayMode = default;
			try
			{
				_inputHostManagerBroker.Instance.GetIhmLocation(out var ret, out displayMode);
			}
			catch(Exception e)
			{
				CreateInputHostManagerBroker();
				error = true;
			}
			if (error)
			{
				_inputHostManagerBroker.Instance.GetIhmLocation(out var ret, out displayMode);
			}
			
			return (displayMode == DisplayMode.Docked) || (displayMode == DisplayMode.Floating);

		}

		private static void StartTabTip()
		{
			ProcessStartInfo psi = new ProcessStartInfo(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe")
			{
				UseShellExecute=true
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
			try
			{
				using (var tipInvocation = ComPtr<ITipInvocation>.Create(ComTypes.TipInvocationType))
				{
					var window = NativeMethods.GetDesktopWindow();
					tipInvocation.Instance.Toggle(NativeMethods.GetDesktopWindow());
				}
			}
			catch(Exception e)
			{

			}
		}

		public override void Show()
		{
			// Detecting TabTip process presence
			if (NativeMethods.IsTabTipProcessPresent())
			{
				if (!IsOpened())
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
