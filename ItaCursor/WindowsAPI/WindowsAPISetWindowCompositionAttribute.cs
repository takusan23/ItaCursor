using System;
using System.Runtime.InteropServices;

namespace ItaCursor.WindowsAPI
{
    /// <summary>
    /// WindowsのAPIに、SetWindowCompositionAttributeっていう非公開APIがあるんだけど、
    /// 
    /// これを使うとウィンドウを半透明にぼかしを書けることができる、
    /// 
    /// https://sourcechord.hatenablog.com/entry/2017/12/01/235228
    /// </summary>
    class WindowsAPISetWindowCompositionAttribute
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        public enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        public enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public uint GradientColor;
            public int AnimationId;
        }


        [DllImport("user32.dll")]
        public static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    }
}
