using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Osklib.Wpf
{
	public static class OnScreenKeyboardSettings
	{
		// There are not much controls which need on-screen keyboard <32

		/// <summary>
		/// Enabled global handlers bitfield
		/// </summary>
		private static StandardControlFlag _enabledBitField =StandardControlFlag.None;
		/// <summary>
		/// Registered EventManager Class Handlers bitfield
		/// </summary>
		private static StandardControlFlag _registeredHandlers = 0;
		private static EventHandler<TouchEventArgs> _touchHandler;

		private static Type[] _controlTypes =
		{
			typeof(TextBox),
			typeof(PasswordBox),
			typeof(RichTextBox)
		};


		private static void RegisterTouchHandlerForClass<Type>()
			where Type : UIElement
		{
			if (_touchHandler == null)
			{
				_touchHandler = new EventHandler<TouchEventArgs>(TouchUpEventHandler);
			}
			EventManager.RegisterClassHandler(typeof(Type), UIElement.TouchUpEvent, _touchHandler);
		}

		private const int TextBoxFlag = 0b1;
		private const int PasswordBoxFlag = 0b10;
		private const int RichTextBoxFlag = 0b100;

		public static bool EnableForTextBoxes
		{
			get { return _enabledBitField.HasFlag(StandardControlFlag.TextBox); }
			set
			{
				if (_enabledBitField.HasFlag(StandardControlFlag.TextBox) == value)
				{
					return;
				}
				_enabledBitField ^= StandardControlFlag.TextBox;
				if ((!_registeredHandlers.HasFlag(StandardControlFlag.TextBox)) && value)
				{
					RegisterTouchHandlerForClass<TextBox>();
				}
			}
		}

		public static bool EnableForPasswordBoxes
		{
			get { return _enabledBitField.HasFlag(StandardControlFlag.PasswordBox); }
			set
			{
				if (_enabledBitField.HasFlag(StandardControlFlag.PasswordBox) == value)
				{
					return;
				}
				_enabledBitField ^= StandardControlFlag.PasswordBox;
				if ((!_registeredHandlers.HasFlag(StandardControlFlag.PasswordBox)) && value)
				{
					RegisterTouchHandlerForClass<PasswordBox>();
				}
			}
		}

		public static bool EnableForRichTextBoxes
		{
			get { return _enabledBitField.HasFlag(StandardControlFlag.RichTextBox); }
			set
			{
				if (_enabledBitField.HasFlag(StandardControlFlag.RichTextBox) == value)
				{
					return;
				}
				_enabledBitField ^= StandardControlFlag.RichTextBox;
				if ((!_registeredHandlers.HasFlag(StandardControlFlag.RichTextBox)) && value)
				{
					RegisterTouchHandlerForClass<RichTextBox>();
				}
			}
		}

		private static void TouchUpEventHandler(object sender, TouchEventArgs e)
		{
			// if none active - return;
			if (_enabledBitField == 0)
			{
				return;
			}
			if (sender == null)
			{
				return;
			}
			var type = sender.GetType();

			bool show = false;

			for (int i = 0; i < _controlTypes.Length; i++)
			{
				StandardControlFlag mask = (StandardControlFlag)(1 << i);
				if ((_enabledBitField & mask) != 0)
				{
					if (_controlTypes[i].IsAssignableFrom(type))
					{
						show = true;
						break;
					}
				}
			}

			if (show)
			{
				Osklib.OnScreenKeyboard.Show();
			}
		}



		public static DependencyProperty IsEnabledProperty =
			DependencyProperty.RegisterAttached(
				"IsEnabled",
				typeof(bool),
				typeof(OnScreenKeyboardSettings),
				new PropertyMetadata(false, IsEnabledPropertyChanged));

		private static void IsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			bool oldValue = (bool)e.OldValue;
			bool newValue = (bool)e.NewValue;
			if (oldValue == newValue)
			{
				return;
			}
			UIElement element = (UIElement)d;
			if (newValue)
			{
				element.TouchUp += Element_TouchUp;
			}
			else
			{
				element.TouchUp -= Element_TouchUp;
			}
		}

		private static void Element_TouchUp(object sender, TouchEventArgs e)
		{
			OnScreenKeyboard.Show();
		}

		public static void SetIsEnabled(UIElement element, bool value)
		{
			element.SetValue(IsEnabledProperty, value);
		}

		public static bool GetIsEnabled(UIElement element)
		{
			return (bool)element.GetValue(IsEnabledProperty);
		}
	}
}
