using System.Diagnostics;

namespace ItaCursor.Tool
{
    /// <summary>
    /// このアプリの状態を保存して次回起動時に引き継ぐ。
    /// 
    /// 保存処理をこのクラスへ譲渡する。
    /// </summary>
    class ItaCursorStateDelegateTool
    {

        /// <summary>
        /// 保存する
        /// </summary>
        /// <param name="window">譲渡元</param>
        public static void SaveState(MainWindow window)
        {
            Properties.Settings.Default.IsTouchPadDisable = !window.mouseHook.IsEnable;
            Properties.Settings.Default.MouseButtonHeight = window.RightClickArea.Height;
            // 忘れない
            Properties.Settings.Default.Save();
        }


        /// <summary>
        /// 保存した内容を復元する処理
        /// </summary>
        /// <param name="window">譲渡元</param>
        public static void RestoreState(MainWindow window)
        {
            if (Properties.Settings.Default.MouseButtonHeight != -1)
            {
                window.SetTouchPadEnable(!Properties.Settings.Default.IsTouchPadDisable);
                window.RightClickArea.Height = Properties.Settings.Default.MouseButtonHeight;
            }
        }

    }
}
