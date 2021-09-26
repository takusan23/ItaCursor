using System;
using System.Windows;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPIMonitorFromPoint
    {

        /// <summary>
        /// 指定した点に最も近いモニターハンドルを返す
        /// </summary>
        /// <param name="point">点</param>
        /// <returns>モニターハンドル</returns>
        public static IntPtr GetMonitorHandleFromPoint(Point point)
        {
            // 構造体を作成
            var pointStruct = new WindowsAPI.WindowsAPIMonitorFromPoint.POINT
            {
                x = (int)point.X,
                y = (int)point.Y
            };
            return WindowsAPI.WindowsAPIMonitorFromPoint.MonitorFromPoint(pointStruct, WindowsAPI.WindowsAPIMonitorFromPoint.MonitorOptions.MONITOR_DEFAULTTONEAREST);
        }

    }
}
