using System.Windows.Forms;
using Osklib.WinForms.Interop;

namespace Osklib.WinForms
{
	public static class OnScreenKeyboardSettings
	{
		public static void EnableForTextBox(TextBoxBase textBox)
		{
			textBox.MouseUp += TextBox_MouseUp;
		}

		private static void TextBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (NativeMethods.IsTouchEvent())
			{
				Osklib.OnScreenKeyboard.Show();
			}
		}
	}
}
