using System.Runtime.InteropServices;


namespace Osklib.Interop
{

	[StructLayout(LayoutKind.Sequential)]
	internal unsafe
#if USE_REF_STRUCT
			ref
#endif
			struct RTL_OSVERSIONINFOEXW
	{
		public uint dwOSVersionInfoSize;
		public int dwMajorVersion;
		public int dwMinorVersion;
		public int dwBuildNumber;
		public uint dwPlatformId;
		public fixed char szCSDVersion[128];
		public ushort wServicePackMajor;
		public ushort wServicePackMinor;
		public ushort wSuiteMask;
		public byte wProductType;
		public byte wReserved;
	}
}
