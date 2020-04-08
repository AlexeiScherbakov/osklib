using System;
using System.Diagnostics;
using System.Threading;

using Osklib.Interop;

namespace Osklib
{
	internal sealed class ComOnScreenKeyboardController
		: OnScreenKeyboardController
	{
		private ComPtr<IInputHostManagerBroker> _inputHostManagerBroker;


		public ComOnScreenKeyboardController()
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
			if (IsOpened())
			{
				ToggleByCom();
				return true;
			}
			return false;
		}

		public override bool IsOpened()
		{
			bool error = false;
			DisplayMode displayMode = default;
			try
			{
				_inputHostManagerBroker.Instance.GetIhmLocation(out var ret, out displayMode);
			}
			catch (Exception e)
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
			try
			{
				using (var tipInvocation = ComPtr<ITipInvocation>.Create(ComTypes.TipInvocationType))
				{
					var window = NativeMethods.GetDesktopWindow();
					tipInvocation.Instance.Toggle(NativeMethods.GetDesktopWindow());
				}
			}
			catch (Exception e)
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
				if (!IsOpened())
				{
					ToggleByCom();
				}
			}
		}
	}
}
