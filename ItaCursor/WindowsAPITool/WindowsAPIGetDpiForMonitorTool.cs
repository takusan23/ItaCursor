
using System.Diagnostics;
using System.Windows;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPIGetDpiForMonitorTool
    {

        /// <summary>
        /// 倍率を取得する。DPI取得後96なら100%で表示しているみたいな
        /// </summary>
        /// <param name="point">取得したい画面にマウスポインタを持っていって、その座標</param>
        /// <returns>単位はぱーせんと</returns>
        public static int GetMonitorScalePercentFromPoint(Point point)
        {
            // モニターハンドル取得
            var monitorHandle = WindowsAPIMonitorFromPoint.GetMonitorHandleFromPoint(point);
            // DPI取得
            uint dpiX = 1;
            uint dpiY = 1;
            WindowsAPI.WindowsAPIGetDpiForMonitor.GetDpiForMonitor(monitorHandle, WindowsAPI.WindowsAPIGetDpiForMonitor.MonitorDpiType.Default, ref dpiX, ref dpiY);
            return (int)((dpiX / 96f) * 100); // 96dpi = 100%
        }

    }
}
