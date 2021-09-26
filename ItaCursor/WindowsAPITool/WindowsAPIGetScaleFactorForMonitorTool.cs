using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPIGetScaleFactorForMonitorTool
    {

        /// <summary>
        /// Windowsの設定にある、「テキスト、アプリ、その他の項目のサイズを変更する」の値を取得する
        /// </summary>
        /// <param name="point">カーソル位置。カーソル位置のウィンドウの倍率を出す。</param>
        /// <returns></returns>
        public static float GetMonitorScale(Point point)
        {
            var enumToScale = new Dictionary<WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR, float>();
/*            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_100_PERCENT, 1f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_120_PERCENT, 1.2f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_125_PERCENT, 1.25f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_140_PERCENT, 1.4f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_150_PERCENT, 1.5f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_160_PERCENT, 1.6f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_175_PERCENT, 1.75f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_180_PERCENT, 1.8f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_200_PERCENT, 2f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_225_PERCENT, 2.25f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_250_PERCENT, 2.30f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_300_PERCENT, 3f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_350_PERCENT, 3.5f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_400_PERCENT, 4f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_450_PERCENT, 4.5f);
            enumToScale.Add(WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR.SCALE_500_PERCENT, 5f);
*/            // モニターハンドルを取得
            var monitorHandle = WindowsAPIMonitorFromPoint.GetMonitorHandleFromPoint(point);
            // 倍率を取得する。入試かな？
            WindowsAPI.WindowsAPIGetScaleFactorForMonitor.DEVICE_SCALE_FACTOR dEVICE_SCALE_FACTOR;
            int scale = 100;
            WindowsAPI.WindowsAPIGetScaleFactorForMonitor.GetScaleFactorForMonitor(monitorHandle, out scale);
            Debug.WriteLine(scale);
            return 1f;
        }

    }
}
