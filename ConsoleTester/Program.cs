using System;
using System.Threading;

using Osklib;

namespace ConsoleTester
{
	class Program
	{
		static void Main( string[] args )
		{
			OnScreenKeyboardWatcher watcher = new OnScreenKeyboardWatcher();

			watcher.KeyboardOpened += delegate
			{
				Console.WriteLine("event fired - keyboard opened");
			};
			watcher.KeyboardClosed += delegate
			{
				Console.WriteLine("event fired - keyboard closed");
			};


			bool isOpened = OnScreenKeyboard.IsOpened();
			Console.WriteLine( "Was opened {0}", isOpened );
			OnScreenKeyboard.Show();
			Thread.Sleep( 5000 );
			isOpened = OnScreenKeyboard.IsOpened();
			Console.WriteLine( "Now is opened {0}", isOpened );
			bool closed = OnScreenKeyboard.Close();
			Console.WriteLine( "Was closed {0}", closed );

			Console.ReadLine();
		}
	}
}
