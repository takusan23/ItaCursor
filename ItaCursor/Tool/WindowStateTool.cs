using System.Windows;

namespace ItaCursor.Tool
{
    /// <summary>
    /// ウィンドウの位置を永続化して、次回起動時に引き継ぐ
    /// </summary>
    class WindowStateTool
    {

        /// <summary>
        /// ウィンドウの位置、大きさを保存する
        /// </summary>
        public static void SaveWindowState(Window window)
        {
            Properties.Settings.Default.LastWindowPosX = (int)window.Left;
            Properties.Settings.Default.LastWindowPosY = (int)window.Top;
            Properties.Settings.Default.LastWindowWidth = (int)window.Width;
            Properties.Settings.Default.LastWindowHeight = (int)window.Height;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// ウィンドウの位置、大きさを復元する
        /// </summary>
        public static void RestoreWindowState(Window window)
        {
            if (Properties.Settings.Default.LastWindowHeight != -1 && Properties.Settings.Default.LastWindowWidth != -1)
            {
                // ２回目以降のみ
                window.Left = Properties.Settings.Default.LastWindowPosX;
                window.Top = Properties.Settings.Default.LastWindowPosY;
                window.Width = Properties.Settings.Default.LastWindowWidth;
                window.Height = Properties.Settings.Default.LastWindowHeight;
            }
        }

    }
}
