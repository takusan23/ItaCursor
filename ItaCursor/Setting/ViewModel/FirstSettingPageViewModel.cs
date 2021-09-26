using System.Diagnostics;

namespace ItaCursor.Setting.ViewModel
{
    /// <summary>
    /// 設定画面のViewModel。
    /// 
    /// Androidだと ViewModel+DataBinding
    /// Vue.jsだと 双方向バインディング
    /// 
    /// みたいな感じ？この例だとチェックボックスと値を繋げば連動して変更してくれる。
    /// 
    /// <code>
    ///  CheckBox Margin="5" Content="アクリル効果を利用する（半透明、ぼかし効果）" IsChecked="{Binding IsAcryilc}" Name="AcrylicCheckBox"
    /// </code>
    /// 
    /// 
    /// </summary>
    /// 
    class FirstSettingPageViewModel
    {

        /// <summary>
        /// 半透明にするか
        /// </summary>
        public bool IsOpacity
        {
            get => Properties.Settings.Default.IsOpacity;
            set
            {
                Properties.Settings.Default.IsOpacity = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// アクリル効果を有効化するか
        /// </summary>
        public bool IsAcryilc
        {
            get => Properties.Settings.Default.IsAcryilc;
            set
            {
                Properties.Settings.Default.IsAcryilc = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// クリック遅延
        /// </summary>
        public int ClickDelayMs
        {
            get => Properties.Settings.Default.ClickDelayMs;
            set
            {
                Properties.Settings.Default.ClickDelayMs = value;
                Properties.Settings.Default.Save();
            }
        }

        public int ScreenShotDelayMs
        {
            get => Properties.Settings.Default.ScreenShotDelayMs;
            set
            {
                Properties.Settings.Default.ScreenShotDelayMs = value;
                Properties.Settings.Default.Save();
            }
        }

    }

}
