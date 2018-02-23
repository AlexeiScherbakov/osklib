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
	internal abstract class OnScreenKeyboardController
	{
		public abstract void Show();
		public abstract bool IsOpened();
		public abstract bool Close();
	}
}
