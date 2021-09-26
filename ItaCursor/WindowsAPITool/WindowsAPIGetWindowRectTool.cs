using System;
using System.Windows;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPIGetWindowRectTool
    {
        /// <summary>
        /// 引数のウィンドウ（ハンドル）の位置を返す。Win32APIを叩いてます。（Window.Leftがなんか違う値を返す。解像度のせいかな？）
        /// </summary>
        public static Point GetWindowPos(IntPtr windowHandle)
        {
            WindowsAPI.WindowsAPIGetWindowRect.RECT rect;
            WindowsAPI.WindowsAPIGetWindowRect.GetWindowRect(windowHandle, out rect);
            return new Point(rect.left, rect.top);
        }

        /// <summary>
        /// 指定したウィンドウの四角形？を返す
        /// </summary>
        /// <param name="windowHandle"></param>
        /// <returns></returns>
        public static Rect GetWindowRect(IntPtr windowHandle)
        {
            WindowsAPI.WindowsAPIGetWindowRect.RECT rect;
            WindowsAPI.WindowsAPIGetWindowRect.GetWindowRect(windowHandle, out rect);
            return new Rect(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }

    }
}
