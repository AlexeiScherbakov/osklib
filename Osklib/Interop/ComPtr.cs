using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Osklib.Interop
{
	/// <summary>
	/// WinRT/COM object holder
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal sealed class ComPtr<T>
		: IDisposable
		where T:class
	{
		private T _instance;

		public ComPtr(T pointer)
		{
			_instance = pointer;
		}

		public static implicit operator ComPtr<T>(T pointer)
		{
			return new ComPtr<T>(pointer);
		}

		~ComPtr()
		{
			Free();
		}

		public void Dispose()
		{
			Free();
			GC.SuppressFinalize(this);
		}

		private void Free()
		{
			var ptr = Interlocked.Exchange<T>(ref _instance, null);
			if (null != ptr)
			{
				Marshal.ReleaseComObject(ptr);
			}
		}


		public T Instance
		{
			get { return _instance; }
		}


		public static ComPtr<T> Create(Type comType)
		{
			return (T) Activator.CreateInstance(comType);
		}
	}
}
