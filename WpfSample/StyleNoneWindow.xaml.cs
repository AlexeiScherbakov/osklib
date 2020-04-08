using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Osklib;
using Osklib.Wpf;

namespace WpfSample
{
	/// <summary>
	/// Interaction logic for StyleNoneWindow.xaml
	/// </summary>
	public partial class StyleNoneWindow : Window
	{
		private DispatcherOnScreenKeyboardWatcher _keyBoardWatcher;

		public StyleNoneWindow()
		{
			InitializeComponent();

			_keyBoardWatcher = new DispatcherOnScreenKeyboardWatcher(this.Dispatcher);

			DetectParameters();

			_keyBoardWatcher.ParametersChanged += _keyBoardWatcher_ParametersChanged;
			_keyBoardWatcher.KeyboardOpened += _keyBoardWatcher_KeyboardOpened;
			_keyBoardWatcher.KeyboardClosed += _keyBoardWatcher_KeyboardClosed;
		}

		private void _keyBoardWatcher_ParametersChanged(object sender, EventArgs e)
		{
			DetectParameters();
		}

		private void DetectParameters()
		{
			Osklib.Rect rect = _keyBoardWatcher.Location;
			keyboardLocationTextBox.Text = string.Format("Keyboard position is ({0},{1}-{2},{3}), keyboard display mode is {4}, Is opened - {5}",
					rect.Left, rect.Top, rect.Right, rect.Bottom,
					_keyBoardWatcher.DisplayMode,
					_keyBoardWatcher.IsOpened());
		}


		private void _keyBoardWatcher_KeyboardOpened(object sender, EventArgs e)
		{
			keyboardStateTextBox.Text = "Event: On Screen Keyboard is opened";
		}

		private void _keyBoardWatcher_KeyboardClosed(object sender, EventArgs e)
		{
			keyboardStateTextBox.Text = "Event: On Screen Keyboard is closed";
		}

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			OnScreenKeyboardSettings.EnableForTextBoxes = textBoxToggle.IsChecked == true;
			OnScreenKeyboardSettings.EnableForPasswordBoxes = passwordBoxToggle.IsChecked == true;
			OnScreenKeyboardSettings.EnableForRichTextBoxes = richBoxToggle.IsChecked == true;
		}
	}
}
