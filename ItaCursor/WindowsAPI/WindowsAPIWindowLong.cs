using System;
using System.Runtime.InteropServices;

namespace ItaCursor.WindowsAPI
{
    class WindowsAPIWindowLong
    {
        public static int GWL_EXSTYLE = -20;

        public static uint WS_EX_LAYERED = 0x00080000;

        public static uint WS_EX_TRANSPARENT = 0x00000020;

        public static uint WS_EX_NOACTIVATE = 0x8000000;

        public static uint WS_EX_TOPMOST = 0x00000008;

        [DllImport("user32.dll")]
        public static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
    }
}
