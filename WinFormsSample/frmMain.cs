using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Osklib.WinForms;

namespace WinFormsSample
{
	public partial class frmMain
		: Form
	{
		WinFormsKeyboardWatcher watcher;

		public frmMain()
		{
			InitializeComponent();

			watcher = new WinFormsKeyboardWatcher();
			watcher.KeyboardOpened += Watcher_KeyboardOpened;
			watcher.KeyboardClosed += Watcher_KeyboardClosed;

			OnScreenKeyboardSettings.EnableForTextBox(textBox1);
		}

		private void Watcher_KeyboardClosed(object sender, EventArgs e)
		{
			this.toolStripLabelOskState.Text = "Keyboard is closed";
		}

		private void Watcher_KeyboardOpened(object sender, EventArgs e)
		{
			this.toolStripLabelOskState.Text = "Keyboard is opened";
		}
	}
}
