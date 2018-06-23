using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Osklib.Wpf;

namespace WpfSample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
		: Window
	{
		private DispatcherOnScreenKeyboardWatcher _keyBoardWatcher;

		public MainWindow()
		{
			InitializeComponent();

			_keyBoardWatcher = new DispatcherOnScreenKeyboardWatcher(this.Dispatcher);


			keyboardStateTextBox.Text = GetKeyboardState(_keyBoardWatcher.State);

			_keyBoardWatcher.KeyboardOpened += _keyBoardWatcher_KeyboardOpened;
			_keyBoardWatcher.KeyboardClosed += _keyBoardWatcher_KeyboardClosed;
		}

		private string GetKeyboardState(bool? state)
		{
			if (state.HasValue)
			{
				if (state.Value)
				{
					return "On Screen Keyboard is opened";
				}
				else
				{
					return "On Screen Keyboard is closed";
				}
			}
			return "On Screen Keyboard State is unknown";
		}

		private void _keyBoardWatcher_KeyboardOpened(object sender, EventArgs e)
		{
			keyboardStateTextBox.Text = "On Screen Keyboard is opened";
		}

		private void _keyBoardWatcher_KeyboardClosed(object sender, EventArgs e)
		{
			keyboardStateTextBox.Text = "On Screen Keyboard is closed";
		}

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			OnScreenKeyboardSettings.EnableForTextBoxes = textBoxToggle.IsChecked == true;
			OnScreenKeyboardSettings.EnableForPasswordBoxes = passwordBoxToggle.IsChecked == true;
			OnScreenKeyboardSettings.EnableForRichTextBoxes = richBoxToggle.IsChecked == true;
		}
	}
}
