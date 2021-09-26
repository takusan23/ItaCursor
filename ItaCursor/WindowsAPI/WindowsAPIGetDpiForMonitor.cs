using System;
using System.Runtime.InteropServices;

namespace ItaCursor.WindowsAPI
{
    class WindowsAPIGetDpiForMonitor
    {
        [DllImport("SHCore.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, ref uint dpiX, ref uint dpiY);

        public enum MonitorDpiType { Effective, Angular, Raw, Default = Effective }

    }
}
