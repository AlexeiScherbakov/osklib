using System;
using System.Runtime.InteropServices;

namespace Osklib.Interop
{
	[ComImport]
	[Guid("2166ee67-71df-4476-8394-0ced2ed05274")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IInputHostManagerBroker
	{
		void GetIhmLocation(ref Rect rect, out DisplayMode mode);
	}
}
