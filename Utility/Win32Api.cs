using System;
using System.Runtime.InteropServices;

namespace Utility
{
	internal class Win32Api
	{
		public const int SW_SHOWNORMAL = 1;

		public const int SW_RESTORE = 9;

		public const int SW_SHOWNOACTIVATE = 4;

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		public static extern bool ShowWindow(System.IntPtr hWnd, uint nCmdShow);

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		public static extern System.IntPtr FindWindow(string lpClassName, string lpWindowName);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(System.IntPtr hWnd);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern System.IntPtr SetActiveWindow(System.IntPtr hWnd);
	}
}
