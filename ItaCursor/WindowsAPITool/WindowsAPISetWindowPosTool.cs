using System;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPISetWindowPosTool
    {

        /// <summary>
        /// Win32APIの力で常に最前面にウィンドウを表示させる
        /// </summary>
        /// <param name="windowHandle">ウィンドウハンドル</param>
        public static void SetAlwaysTopWindow(IntPtr windowHandle)
        {
            // 最前面表示
            var flags = WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_NOMOVE | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_NOSIZE | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_NOOWNERZORDER | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_FRAMECHANGED | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_NOSENDCHANGING | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_NOACTIVATE | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_SHOWWINDOW;
            WindowsAPI.WindowsAPISetWindowPos.SetWindowPos(windowHandle, WindowsAPI.WindowsAPISetWindowPos.HWND_TOPMOST, 0, 0, 0, 0, flags);
        }

        /// <summary>
        /// 最前面表示 + 指定位置に移動　する関数。
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public static void SetWindowPos(IntPtr windowHandle, int x, int y)
        {
            // 最前面表示
            var flags = WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_NOSIZE | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_NOOWNERZORDER | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_FRAMECHANGED | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_NOSENDCHANGING | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_NOACTIVATE | WindowsAPI.WindowsAPISetWindowPos.SetWindowPosFlags.SWP_SHOWWINDOW;
            WindowsAPI.WindowsAPISetWindowPos.SetWindowPos(windowHandle, WindowsAPI.WindowsAPISetWindowPos.HWND_TOPMOST, x, y, 0, 0, flags);
        }

    }
}
