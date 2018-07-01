using System;

namespace Osklib.Wpf
{
	[Flags]
	internal enum StandardControlFlag
	{
		None,
		TextBox = 0b1,
		PasswordBox = 0b10,
		RichTextBox = 0b100
	}
}
