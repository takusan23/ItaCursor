using System;
using System.Runtime.InteropServices;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPIToolWindowsAPISetWindowCompositionAttribute
    {

        /// <summary>
        /// ウィンドウを半透明にしてアクリル効果（ぼかし）をつける
        /// </summary>
        /// <param name="windowHandle">ウィンドウハンドル</param>
        /// <param name="isEnable">有効にする際はtrue</param>
        public static void SetWindowAcryilc(IntPtr windowHandle, bool isEnable = true)
        {
            var accent = new WindowsAPI.WindowsAPISetWindowCompositionAttribute.AccentPolicy();
            var accentStructSize = Marshal.SizeOf(accent);
            if (isEnable)
            {
                accent.AccentState = WindowsAPI.WindowsAPISetWindowCompositionAttribute.AccentState.ACCENT_ENABLE_BLURBEHIND;
                accent.AccentFlags = 2;
                accent.GradientColor = 0x7FFFFFFF;
            }

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowsAPI.WindowsAPISetWindowCompositionAttribute.WindowCompositionAttributeData();
            data.Attribute = WindowsAPI.WindowsAPISetWindowCompositionAttribute.WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            WindowsAPI.WindowsAPISetWindowCompositionAttribute.SetWindowCompositionAttribute(windowHandle, ref data);
        }

    }
}
