using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ItaCursor.ToolWindow
{
    /// <summary>
    /// VolumeControlWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class VolumeControlWindow : Window
    {
        public VolumeControlWindow()
        {
            InitializeComponent();

            // ウィンドウハンドル取得のための
            Loaded += (s, e) =>
            {
                var windowHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
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
            };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VolumeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}