using System;
using System.Threading;
using System.Windows.Threading;

namespace Osklib.Wpf
{
	public sealed class DispatcherOnScreenKeyboardWatcher
		: BaseEventOnScreenKeyboardWatcher
	{
		private DispatcherTimer _timer;


		public DispatcherOnScreenKeyboardWatcher(Dispatcher dispatcher)
			: this(dispatcher, DispatcherPriority.Background)
		{

		}

		public DispatcherOnScreenKeyboardWatcher(Dispatcher dispatcher, DispatcherPriority dispatcherPriority)
		{
			CheckKeyboard();
			_timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100), dispatcherPriority, TimerTick, dispatcher);
			_timer.Start();
		}

		protected override void Dispose(bool disposing)
		{
			_timer.Stop();
			base.Dispose(disposing);
		}

		private void TimerTick(object sender, EventArgs e)
		{
			CheckKeyboard();
		}
	}
}
