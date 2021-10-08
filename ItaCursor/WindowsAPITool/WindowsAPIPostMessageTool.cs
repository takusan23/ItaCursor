using ItaCursor.WindowsAPI;
using System;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPIPostMessageTool
    {

        public static void PostScroll(int x, int y,int scroll)
        {
            // 指定地点のウィンドウハンドル
            var handle = WindowsAPIWindowFromPointTool.GetWindowHandleFromPos(x, y);
            // https://social.msdn.microsoft.com/Forums/vstudio/ja-JP/42c2257a-f589-448d-9723-0dc2af95a038/12502125211245412470123981247312463125251254012523124961254012?forum=csharpgeneralja
            var WHEEL_DELTA = 120;
            uint WM_MOUSEWHEEL = 0x20A;
            var wParam = new IntPtr((WHEEL_DELTA * scroll) << 16);
            var lParam = new IntPtr(((y & 0xFFFF) << 16) | (x & 0xFFFF));
            WindowsAPIPostMessage.PostMessage(handle, WM_MOUSEWHEEL, wParam, lParam);
        }

    }
}
