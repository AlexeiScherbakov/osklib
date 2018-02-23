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

	internal sealed class UnsupportedOsOnScreenKeyboardController
		: OnScreenKeyboardController
	{
		public override bool Close()
		{
			return false;
		}

		public override bool IsOpened()
		{
			return false;
		}

		public override void Show()
		{
			
		}
	}
}
