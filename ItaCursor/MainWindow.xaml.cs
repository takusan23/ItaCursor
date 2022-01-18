using ItaCursor.Setting;
using ItaCursor.Tool;
using ItaCursor.ToolWindow;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
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
        public WindowsAPITool.WindowsAPISetWindowsHookExTool mouseHook { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            // カーソル用ウィンドウ表示
            virtualCursorWindow.Show();

            // Windows10のアクセントカラーに設定する
            ParentBorder.Background = new SolidColorBrush(WindowsAccentColor.GetAccentColor().GetValueOrDefault(Colors.Transparent));

            // ウィンドウハンドル取得のための
            Loaded += (s, e) =>
            {
                var windowHandle = new WindowInteropHelper(this).Handle;

                // 本命
                mouseHook = new WindowsAPITool.WindowsAPISetWindowsHookExTool(windowHandle, new WindowInteropHelper(virtualCursorWindow).Handle);

                // TopMost
                WindowsAPITool.WindowsAPISetWindowPosTool.SetAlwaysTopWindow(windowHandle);
                // このアプリを選択してもアクティブ状態にしない
                WindowsAPITool.WindowsAPIWindowLongTool.SetDisableWindow(windowHandle);

                // 半透明にする設定？
                if (Properties.Settings.Default.IsOpacity)
                {
                    Opacity = Properties.Settings.Default.WindowOpacity;
                }

                // アクリル効果をつける
                if (Properties.Settings.Default.IsAcryilc)
                {
                    WindowsAPITool.WindowsAPIToolWindowsAPISetWindowCompositionAttribute.SetWindowAcryilc(windowHandle);
                    // 角丸使えないので戻す
                    ParentBorder.CornerRadius = new CornerRadius(0);
                    // 移動中はアクリル効果を切る
                    AppBar.MouseLeftButtonDown += (s, e) => { WindowsAPITool.WindowsAPIToolWindowsAPISetWindowCompositionAttribute.SetWindowAcryilc(windowHandle, false); };
                    // 離したら戻す。なぜかAppBarではMouseUpが拾えない
                    ParentBorder.MouseLeftButtonUp += (s, e) => { WindowsAPITool.WindowsAPIToolWindowsAPISetWindowCompositionAttribute.SetWindowAcryilc(windowHandle, true); };
                }

                // 範囲を設定
                SetTrackPadArea();
                SetScrollBarArea();
                SetClickButtonArea();

                // ウィンドウ状態を復元
                WindowStateTool.RestoreWindowState(this);
                // ボタンの高さとかを復元
                ItaCursorStateDelegateTool.RestoreState(this);

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
                    // ボタンの高さとか保存
                    ItaCursorStateDelegateTool.SaveState(this);
                };

                SizeChanged += (s, e) =>
                {
                    // サイズ変更した
                    SetTrackPadArea();
                    SetScrollBarArea();
                    SetClickButtonArea();
                };

                LocationChanged += (s, e) =>
                {
                    // ウィンドウ移動した
                    SetTrackPadArea();
                    SetScrollBarArea();
                    SetClickButtonArea();
                };

                // ウィンドウを移動できるように
                AppBar.MouseLeftButtonDown += (s, e) => { DragMove(); };
            };

        }

        /// <summary>
        /// タッチパネル操作を有効、無効にする
        /// </summary>
        /// <param name="IsEnable">有効にするならtrue</param>
        public void SetTouchPadEnable(bool IsEnable)
        {
            mouseHook.IsEnable = IsEnable;
            EnableSwitchText.Text = IsEnable ? "\xeda4" : "\xf140";
            MouseClickGridSplitter.IsEnabled = !IsEnable;
            TouchPadDisableText.Visibility = IsEnable ? Visibility.Hidden : Visibility.Visible;
            ResizeMode = IsEnable ? ResizeMode.NoResize : ResizeMode.CanResizeWithGrip;
        }

        /// <summary>
        /// WindowsAPISetWindowsHookExToolにトラックパッドの範囲を設定する
        /// </summary>
        private void SetTrackPadArea() => mouseHook.SetTrackPadRectArea(CreateRectFromControl(TouchPadArea));

        /// <summary>
        /// スクロールバーの範囲を設定する
        /// </summary>
        private void SetScrollBarArea() => mouseHook.SetScrollBarRectArea(CreateRectFromControl(ScrollArea));

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
        /// Controlからスクリーン座標な四角形を返す。拡大率に対応済み
        /// </summary>
        private Rect CreateRectFromControl(Decorator control)
        {
            // ディスプレイの拡大率の取得。マウスポインタの座標にあるディスプレイの拡大率
            WindowsAPI.WindowsAPICursor.POINT _currentCursorPos;
            WindowsAPI.WindowsAPICursor.GetCursorPos(out _currentCursorPos);
            var percent = WindowsAPITool.WindowsAPIGetDpiForMonitorTool.GetMonitorScalePercentFromPoint(new Point(_currentCursorPos.X, _currentCursorPos.Y)) / 100f;
            // 拡大率をかけて返す
            var point = control.PointToScreen(new Point(0, 0));
            return new Rect(point.X, point.Y, control.ActualWidth * percent, control.ActualHeight * percent);
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

        /// <summary>
        /// 有効、無効化ボタン
        /// </summary>
        private void EnableSwitch_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetTouchPadEnable(!mouseHook.IsEnable);
        }
    }
}
