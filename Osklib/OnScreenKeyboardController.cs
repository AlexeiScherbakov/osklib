namespace Osklib
{
	/// <summary>
	/// Base class for On Screen Keyboard Controllers for different OSes
	/// </summary>
	internal abstract class OnScreenKeyboardController
	{
		public abstract void Show();
		public abstract bool IsOpened();
		public abstract bool Close();
	}
}
