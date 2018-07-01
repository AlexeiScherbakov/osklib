using System;
using System.Runtime.InteropServices;

namespace Osklib.Interop
{
	[ComImport]
	[Guid("9767060c-9476-42e2-8f7b-2f10fd13765c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IImmersiveShellBroker
	{
		void Dummy();
		IInputHostManagerBroker GetInputHostManagerBroker();
	}
}
