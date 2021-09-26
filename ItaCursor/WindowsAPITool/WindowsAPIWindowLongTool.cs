using System;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPIWindowLongTool
    {
        /// <summary>
        /// クリックイベントを透過するウィンドウにする
        /// </summary>
        /// <param name="windowHandle">透過させたいウィンドウのウィンドウハンドル</param>
        public static void SetWindowClickTransparent(IntPtr windowHandle)
        {
            var style = WindowsAPI.WindowsAPIWindowLong.GetWindowLong(windowHandle, WindowsAPI.WindowsAPIWindowLong.GWL_EXSTYLE);
            WindowsAPI.WindowsAPIWindowLong.SetWindowLong(windowHandle, WindowsAPI.WindowsAPIWindowLong.GWL_EXSTYLE, style | WindowsAPI.WindowsAPIWindowLong.WS_EX_TRANSPARENT);
        }

        /// <summary>
        /// ウィンドウを非アクティブ状態にする
        /// </summary>
        /// <param name="windowHandle">非アクティブさせたいウィンドウのウィンドウハンドル</param>
        public static void SetDisableWindow(IntPtr windowHandle)
        {
            var style = WindowsAPI.WindowsAPIWindowLong.GetWindowLong(windowHandle, WindowsAPI.WindowsAPIWindowLong.GWL_EXSTYLE);
            WindowsAPI.WindowsAPIWindowLong.SetWindowLong(windowHandle, WindowsAPI.WindowsAPIWindowLong.GWL_EXSTYLE, style | WindowsAPI.WindowsAPIWindowLong.WS_EX_NOACTIVATE | WindowsAPI.WindowsAPIWindowLong.WS_EX_TOPMOST);
        }

    }
}
