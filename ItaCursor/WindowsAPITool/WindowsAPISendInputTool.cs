using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

/// <summary>
/// WindowsAPISentInput を使いやすくしたもの。
/// </summary>
namespace ItaCursor.WindowsAPITool
{
    class WindowsAPISendInputTool
    {

        /// <summary>
        /// SetWindowHookExでクリックイベントを拾ってしまうので、関数でクリックイベントを発生させた場合はこの値を ExtraInfo に詰めてます。
        /// </summary>
        public static IntPtr MOUSE_CLICK_EXTRA_INFO = new IntPtr(2525);

        /// <summary>
        /// クリックする関数。
        /// </summary>
        /// <param name="isRightClick">右クリックならtrue。左ならfalse</param>
        public static void SendClick(bool isRightClick)
        {
            // C#でもなんかKotlinのifが式みたいなやつがあったわ
            int clickDownFlags = isRightClick ? WindowsAPI.WindowsAPISendInput.MOUSEEVENTF_RIGHTDOWN : WindowsAPI.WindowsAPISendInput.MOUSEEVENTF_LEFTDOWN;
            int clickUpFlags = isRightClick ? WindowsAPI.WindowsAPISendInput.MOUSEEVENTF_RIGHTUP : WindowsAPI.WindowsAPISendInput.MOUSEEVENTF_LEFTUP;

            var mouseInputList = new WindowsAPI.WindowsAPISendInput.Input[2];
            mouseInputList[0] = new WindowsAPI.WindowsAPISendInput.Input();
            mouseInputList[0].Type = WindowsAPI.WindowsAPISendInput.INPUT_MOUSE;
            mouseInputList[0].ui = new WindowsAPI.WindowsAPISendInput.InputUnion();
            mouseInputList[0].ui.Mouse.X = 0;
            mouseInputList[0].ui.Mouse.Y = 0;
            mouseInputList[0].ui.Mouse.Flags = clickDownFlags;
            mouseInputList[0].ui.Mouse.ExtraInfo = MOUSE_CLICK_EXTRA_INFO;

            mouseInputList[1] = new WindowsAPI.WindowsAPISendInput.Input();
            mouseInputList[1].Type = WindowsAPI.WindowsAPISendInput.INPUT_MOUSE;
            mouseInputList[1].ui = new WindowsAPI.WindowsAPISendInput.InputUnion();
            mouseInputList[1].ui.Mouse.X = 0;
            mouseInputList[1].ui.Mouse.Y = 0;
            mouseInputList[1].ui.Mouse.Flags = clickUpFlags;
            mouseInputList[1].ui.Mouse.ExtraInfo = MOUSE_CLICK_EXTRA_INFO;

            WindowsAPI.WindowsAPISendInput.SendInput(mouseInputList.Length, mouseInputList, Marshal.SizeOf(mouseInputList[0]));
        }

        /// <summary>
        /// スクリーンショットのショートカットキーを押しに行く。
        /// </summary>
        /// <param name="window">ウィンドウを消す場合は消すウィンドウを指定して。ない場合はnullおｋ</param>
        public static async void SendScreenShotShortcut(Window[]? windowList, int delay = 100)
        {
            if (windowList != null)
            {
                foreach (var window in windowList)
                {
                    window.Hide();
                }
                await Task.Delay(100);
            }

            WindowsAPI.WindowsAPISendInput.Input[] inputs = new WindowsAPI.WindowsAPISendInput.Input[4];

            // Windowsキー押す
            inputs[0] = new WindowsAPI.WindowsAPISendInput.Input();
            inputs[0].Type = 1;
            inputs[0].ui.Keyboard.VirtualKey = WindowsAPI.WindowsAPISendInput.VK_LWIN;
            inputs[0].ui.Keyboard.ScanCode = (short)WindowsAPI.WindowsAPISendInput.MapVirtualKey(WindowsAPI.WindowsAPISendInput.VK_LWIN, 0);
            inputs[0].ui.Keyboard.Flags = WindowsAPI.WindowsAPISendInput.KEYEVENTF_KEYDOWN;
            inputs[0].ui.Keyboard.Time = 0;
            inputs[0].ui.Keyboard.ExtraInfo = IntPtr.Zero;

            // PrintScreenキー押す
            inputs[1] = new WindowsAPI.WindowsAPISendInput.Input();
            inputs[1].Type = 1;
            inputs[1].ui.Keyboard.VirtualKey = WindowsAPI.WindowsAPISendInput.VK_SNAPSHOT;
            inputs[1].ui.Keyboard.ScanCode = (short)WindowsAPI.WindowsAPISendInput.MapVirtualKey(WindowsAPI.WindowsAPISendInput.VK_LWIN, 0);
            inputs[1].ui.Keyboard.Flags = WindowsAPI.WindowsAPISendInput.KEYEVENTF_KEYDOWN;
            inputs[1].ui.Keyboard.Time = 0;
            inputs[1].ui.Keyboard.ExtraInfo = IntPtr.Zero;

            // Windowsキー離す
            inputs[2] = new WindowsAPI.WindowsAPISendInput.Input();
            inputs[2].Type = 1;
            inputs[2].ui.Keyboard.VirtualKey = WindowsAPI.WindowsAPISendInput.VK_LWIN;
            inputs[2].ui.Keyboard.ScanCode = (short)WindowsAPI.WindowsAPISendInput.MapVirtualKey(WindowsAPI.WindowsAPISendInput.VK_LWIN, 0);
            inputs[2].ui.Keyboard.Flags = WindowsAPI.WindowsAPISendInput.KEYEVENTF_KEYUP;
            inputs[2].ui.Keyboard.Time = 0;
            inputs[2].ui.Keyboard.ExtraInfo = IntPtr.Zero;

            // PrintScreenキー離す
            inputs[3] = new WindowsAPI.WindowsAPISendInput.Input();
            inputs[3].Type = 1;
            inputs[3].ui.Keyboard.VirtualKey = WindowsAPI.WindowsAPISendInput.VK_SNAPSHOT;
            inputs[3].ui.Keyboard.ScanCode = (short)WindowsAPI.WindowsAPISendInput.MapVirtualKey(WindowsAPI.WindowsAPISendInput.VK_LWIN, 0);
            inputs[3].ui.Keyboard.Flags = WindowsAPI.WindowsAPISendInput.KEYEVENTF_KEYUP;
            inputs[3].ui.Keyboard.Time = 0;
            inputs[3].ui.Keyboard.ExtraInfo = IntPtr.Zero;

            WindowsAPI.WindowsAPISendInput.SendInput(inputs.Length, inputs, Marshal.SizeOf(inputs[0]));
            await Task.Delay(100);

            if (windowList != null)
            {
                foreach (var window in windowList)
                {
                    window.Show();
                }
            }
        }

    }
}
