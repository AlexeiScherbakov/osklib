using System;
using System.Runtime.InteropServices;

namespace Osklib
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Rect
		: IEquatable<Rect>
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public bool Equals(Rect other)
		{
			return (Left == other.Left) &&
				(Top == other.Top) &&
				(Right == other.Right) &&
				(Bottom == other.Bottom);
		}

		public override bool Equals(object obj)
		{
			if (obj is Rect other)
			{
				return (Left == other.Left) &&
					(Top == other.Top) &&
					(Right == other.Right) &&
					(Bottom == other.Bottom);
			}
			return false;
		}

		public override int GetHashCode()
		{
			var hashCode = -1819631549;
			hashCode = hashCode * -1521134295 + Left.GetHashCode();
			hashCode = hashCode * -1521134295 + Top.GetHashCode();
			hashCode = hashCode * -1521134295 + Right.GetHashCode();
			hashCode = hashCode * -1521134295 + Bottom.GetHashCode();
			return hashCode;
		}
	}
}
