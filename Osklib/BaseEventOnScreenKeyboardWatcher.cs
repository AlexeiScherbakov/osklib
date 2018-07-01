using System;

namespace Osklib
{
	public abstract class BaseEventOnScreenKeyboardWatcher
		: BaseOnScreenKeyboardWatcher, IOnScreenKeyboardEventWatcher
	{
		protected override void OnKeyboardOpened()
		{
			var evnt = KeyboardOpened;
			if (evnt != null)
			{
				evnt(this, EventArgs.Empty);
			}
		}

		protected override void OnKeyboardClosed()
		{
			var evnt = KeyboardClosed;
			if (evnt != null)
			{
				evnt(this, EventArgs.Empty);
			}
		}

		protected override void OnParametersChanged()
		{
			var evnt = ParametersChanged;
			if (evnt != null)
			{
				evnt(this, EventArgs.Empty);
			}
		}

		public event EventHandler ParametersChanged;
		public event EventHandler KeyboardOpened;
		public event EventHandler KeyboardClosed;
	}
}
