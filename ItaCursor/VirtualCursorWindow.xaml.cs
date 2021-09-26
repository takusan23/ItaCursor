using System;
using System.Windows;
using System.Windows.Interop;

namespace ItaCursor
{
    /// <summary>
    /// VirtualCursorWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class VirtualCursorWindow : Window
    {

        /// <summary>
        /// ウィンドウハンドル
        /// </summary>
        private IntPtr windowHandle;

        public VirtualCursorWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                windowHandle = new WindowInteropHelper(this).Handle;
                // クリックの邪魔をしないように、このウィンドウにはクリックイベントを渡さない
                WindowsAPITool.WindowsAPIWindowLongTool.SetWindowClickTransparent(windowHandle);
            };

            // 今のカーソル位置にVirtualCursorWindowを移動させる
            WindowsAPI.WindowsAPICursor.POINT currentPoint;
            WindowsAPI.WindowsAPICursor.GetCursorPos(out currentPoint);
            Left = currentPoint.X;
            Top = currentPoint.Y;

        }

        /// <summary>
        /// このウィンドウの位置を返す
        /// </summary>
        public Point GetVirtualCursorWindowPos()
        {
            var windowHandle = new WindowInteropHelper(this).Handle;
            return WindowsAPITool.WindowsAPIGetWindowRectTool.GetWindowPos(windowHandle);
        }

        /// <summary>
        /// ウィンドウハンドルを返します。
        /// </summary>
        public IntPtr GetWindowHandle() => windowHandle;


    }
}
