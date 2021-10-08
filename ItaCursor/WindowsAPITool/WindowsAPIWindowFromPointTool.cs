using System;
using System.Windows;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPIWindowFromPointTool
    {

        /// <summary>
        /// マウスカーソルが引数のウィンドウにいるか。
        /// </summary>
        /// <param name="window">カーソルがいるか確認したいウィンドウハンドル</param>
        /// <returns>マウスカーソルがこのウィンドウにいたらtrue</returns>
        public static bool IsCursorContains(IntPtr windowHandle)
        {
            // マウスカーソルの位置
            WindowsAPI.WindowsAPICursor.POINT _currentCursorPos;
            WindowsAPI.WindowsAPICursor.GetCursorPos(out _currentCursorPos);

            var currentCursorContainsWindow = WindowsAPI.WindowsAPIWindowFromPoint.WindowFromPoint(_currentCursorPos);
            return windowHandle == currentCursorContainsWindow;
        }

        /// <summary>
        /// 座標を渡してウィンドウハンドルをもらう関数
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>ウィンドウハンドル</returns>
        public static IntPtr GetWindowHandleFromPos(int x, int y)
        {
            return WindowsAPI.WindowsAPIWindowFromPoint.WindowFromPoint(new(x, y));
        }

    }
}
