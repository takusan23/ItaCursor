using System;
using System.Runtime.InteropServices;

namespace ItaCursor.WindowsAPI
{
    class WindowsAPIPostMessage
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
}
