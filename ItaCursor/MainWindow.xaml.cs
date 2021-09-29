using System.Windows;
using System.Windows.Controls;
using ItaCursor.Setting;
using ItaCursor.ToolWindow;
using System.Linq;
using System.Windows.Interop;
using System.Threading;
using ItaCursor.Tool;
using System.Windows.Media;

namespace ItaCursor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// カーソルウィンドウ
        /// </summary>
        private VirtualCursorWindow virtualCursorWindow = new VirtualCursorWindow();

        /// <summary>
        /// カーソル移動するやつ。SetWindowsHookExはここで呼んでる
        /// </summary>
        private WindowsAPITool.WindowsAPISetWindowsHookExTool mouseHook;

        public MainWindow()
        {
            InitializeComponent();

            // カーソル用ウィンドウ表示
            virtualCursorWindow.Show();

            // Windows10のアクセントカラーに設定する
            ParentBorder.Background = new SolidColorBrush(WindowsAccentColor.GetAccentColor().GetValueOrDefault(Colors.Transparent));

            // ウィンドウ状態を復元
            WindowStateTool.RestoreWindowState(this);

            // ウィンドウハンドル取得のための
            Loaded += (s, e) =>
            {
                var windowHandle = new WindowInteropHelper(this).Handle;


                // 半透明にする設定？
                if (Properties.Settings.Default.IsOpacity)
                {
                    Opacity = 0.5;
                }

                // アクリル効果をつける
                if (Properties.Settings.Default.IsAcryilc)
                {
                    WindowsAPITool.WindowsAPIToolWindowsAPISetWindowCompositionAttribute.SetWindowAcryilc(windowHandle);
                }

                // TopMost
                WindowsAPITool.WindowsAPISetWindowPosTool.SetAlwaysTopWindow(windowHandle);
                // このアプリを選択してもアクティブ状態にしない
                WindowsAPITool.WindowsAPIWindowLongTool.SetDisableWindow(windowHandle);

                // 本命
                mouseHook = new WindowsAPITool.WindowsAPISetWindowsHookExTool(windowHandle, new WindowInteropHelper(virtualCursorWindow).Handle);

                // 範囲を設定
                SetTrackPadArea();
                SetClickButtonArea();

                Closed += (s, e) =>
                {
                    // リソース開放
                    virtualCursorWindow.Close();
                    mouseHook.UnhookWindowsHookEx();
                };

                Closing += (s, e) =>
                {
                    // ウィンドウ状態を保存
                    WindowStateTool.SaveWindowState(this);
                };

                SizeChanged += (s, e) =>
                {
                    // サイズ変更した
                    SetTrackPadArea();
                    SetClickButtonArea();
                };

                LocationChanged += (s, e) =>
                {
                    // ウィンドウ移動した
                    SetTrackPadArea();
                    SetClickButtonArea();
                };
            };

        }

        /// <summary>
        /// WindowsAPISetWindowsHookExToolにトラックパッドの範囲を設定する
        /// </summary>
        private void SetTrackPadArea() => mouseHook.SetTrackPadRectArea(CreateRectFromControl(TouchPadArea));

        /// <summary>
        /// WindowsAPISetWindowsHookExToolにカーソルそのままクリックイベントをもらうやつを設定する。
        /// 
        /// これしないと右クリックボタン等を押したときにカーソルが移動してしまう。
        /// </summary>
        private void SetClickButtonArea()
        {
            // 既存のやつは解除
            mouseHook.RemoveAllTouchRect();
            // クリックイベントを登録
            mouseHook.AddTouchRect(CreateRectFromControl(RightClickArea), () =>
            {
                new Thread(() => WindowsAPITool.WindowsAPISendInputTool.SendClick(true)).Start();
            });
            mouseHook.AddTouchRect(CreateRectFromControl(LeftClickArea), () =>
            {
                new Thread(() => WindowsAPITool.WindowsAPISendInputTool.SendClick(false)).Start();
            });
        }


        /// <summary>
        /// Controlからスクリーン座標な四角形を返す
        /// </summary>
        private Rect CreateRectFromControl(Decorator control)
        {
            var point = control.PointToScreen(new Point(0, 0));
            return new Rect(point.X, point.Y, control.ActualWidth, control.ActualHeight);
        }

        /// <summary>
        /// 移動できるように
        /// </summary>
        private void AppBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 終了ボタン
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// スクショボタン押したら
        /// </summary>
        private void ScreenShotButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowsAPITool.WindowsAPISendInputTool.SendScreenShotShortcut(Application.Current.Windows.Cast<Window>().ToArray(), Properties.Settings.Default.ScreenShotDelayMs);
        }

        /// <summary>
        /// 設定ボタンおしたら
        /// </summary>
        private void SettingButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new SettingWindow().Show();
        }

        /// <summary>
        /// 音量コントローラー
        /// </summary>
        private void VolumeButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new VolumeControlWindow().Show();
        }



    }
}
