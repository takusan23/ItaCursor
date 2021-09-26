using System;
using System.Runtime.InteropServices;

namespace ItaCursor.WindowsAPI
{
    class WindowsAPIGetWindowRect
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }
}
