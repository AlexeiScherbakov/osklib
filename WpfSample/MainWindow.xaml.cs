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
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			OnScreenKeyboardSettings.EnableForTextBoxes = textBoxToggle.IsChecked == true;
			OnScreenKeyboardSettings.EnableForPasswordBoxes = passwordBoxToggle.IsChecked == true;
			OnScreenKeyboardSettings.EnableForRichTextBoxes = richBoxToggle.IsChecked == true;
		}
	}
}
