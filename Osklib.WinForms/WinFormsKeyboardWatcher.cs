using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Osklib.Interop;
using Osklib.WinForms.Interop;

namespace Osklib.WinForms
{
	public partial class WinFormsKeyboardWatcher
		: Component, IOnScreenKeyboardEventWatcher
	{
		private OnScreenKeyboardWatcher _watcher;


		#region ctor & dtor
		public WinFormsKeyboardWatcher()
		{
			InitializeComponent();
		}

		public WinFormsKeyboardWatcher(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			var watcher = Interlocked.Exchange(ref _watcher, null);
			if (watcher != null)
			{
				watcher.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			_watcher = new OnScreenKeyboardWatcher(this);
		}
		#endregion

		public DateTime? OpenedSince
		{
			get { return _watcher.OpenedSince; }
		}

		public OnScreenKeyboardDisplayMode DisplayMode
		{
			get { return _watcher.DisplayMode; }
		}

		public Rect Location
		{
			get { return _watcher.Location; }
		}

		#region events

		private void OnParametersChanged()
		{
			var evnt = ParametersChanged;
			if (evnt != null)
			{
				evnt(this, EventArgs.Empty);
			}
		}

		public event EventHandler ParametersChanged;

		private void OnKeyboardOpened()
		{
			var evnt = KeyboardOpened;
			if (evnt != null)
			{
				evnt(this, EventArgs.Empty);
			}
		}
		public event EventHandler KeyboardOpened;


		private void OnKeyboardClosed()
		{
			var evnt = KeyboardClosed;
			if (evnt != null)
			{
				evnt(this, EventArgs.Empty);
			}
		}

		public event EventHandler KeyboardClosed;
		#endregion

		private sealed class OnScreenKeyboardWatcher
			: BaseOnScreenKeyboardWatcher
		{
			private System.Windows.Forms.Timer _timer;
			private WinFormsKeyboardWatcher _watcher;

			public OnScreenKeyboardWatcher(WinFormsKeyboardWatcher watcher)
			{
				_watcher = watcher;

				_timer = new System.Windows.Forms.Timer();
				_timer.Interval = 100;
				_timer.Tick += TimerTick;
				_timer.Enabled = true;
			}

			protected override void Dispose(bool disposing)
			{
				Disposer.Dispose(ref _timer);
				base.Dispose();
			}

			private void TimerTick(object sender, System.EventArgs e)
			{
				CheckKeyboard();
			}
			protected override void OnKeyboardOpened()
			{
				_watcher.OnKeyboardOpened();
			}

			protected override void OnKeyboardClosed()
			{
				_watcher.OnKeyboardClosed();
			}

			protected override void OnParametersChanged()
			{
				_watcher.OnParametersChanged();
			}
		}
	}
}
