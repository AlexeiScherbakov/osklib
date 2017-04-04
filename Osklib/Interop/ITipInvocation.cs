using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Osklib.Interop
{
	[ComImport]
	[Guid( "37c994e7-432b-4834-a2f7-dce1f13b834b" )]
	[InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	interface ITipInvocation
	{
		void Toggle( IntPtr hwnd );
	}
}
