namespace Osklib
{

	internal sealed class UnsupportedOsOnScreenKeyboardController
		: OnScreenKeyboardController
	{
		public override bool Close()
		{
			return false;
		}

		public override bool IsOpened()
		{
			return false;
		}

		public override void Show()
		{
			
		}
	}
}
