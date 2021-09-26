using System;
using System.Runtime.InteropServices;

namespace ItaCursor.WindowsAPI
{
    class WindowsAPIGetMonitorInfo
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct MONITORINFOEX
        {
            public int Size;
            public RectStruct Monitor;
            public RectStruct WorkArea;
            public uint Flags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct RectStruct
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

    }
}
