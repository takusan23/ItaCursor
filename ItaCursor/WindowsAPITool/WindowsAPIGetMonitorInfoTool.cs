using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPIGetMonitorInfoTool
    {
        /// <summary>
        /// 指定したモニターハンドルのモニターを四角形の形にして返す。
        /// </summary>
        /// <param name="monitorHandle">モニターハンドル。GetMonitorHandleFromPoint()参照</param>
        /// <returns>モニターの大きさを四角形の形で</returns>
        public static Rect GetMonitorRectFromMonitorHandle(IntPtr monitorHandle)
        {
            WindowsAPI.WindowsAPIGetMonitorInfo.MONITORINFOEX mONITORINFO = new();
            mONITORINFO.Size = Marshal.SizeOf<WindowsAPI.WindowsAPIGetMonitorInfo.MONITORINFOEX>();
            WindowsAPI.WindowsAPIGetMonitorInfo.GetMonitorInfo(monitorHandle, ref mONITORINFO);
            return new Rect(mONITORINFO.WorkArea.Left, mONITORINFO.WorkArea.Top, mONITORINFO.WorkArea.Right, mONITORINFO.WorkArea.Bottom);
        }
    }
}
