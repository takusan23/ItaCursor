using System;
using System.Runtime.InteropServices;

namespace ItaCursor.WindowsAPI
{
    class WindowsAPIGetScaleFactorForMonitor
    {
        [DllImport("Shcore.dll", CharSet = CharSet.Auto, PreserveSig = false)]
        public static extern IntPtr GetScaleFactorForMonitor(IntPtr hMon, out int pScale);

        public enum DEVICE_SCALE_FACTOR
        {
/*            DEVICE_SCALE_FACTOR_INVALID,
            SCALE_100_PERCENT,
            SCALE_120_PERCENT,
            SCALE_125_PERCENT,
            SCALE_140_PERCENT,
            SCALE_150_PERCENT,
            SCALE_160_PERCENT,
            SCALE_175_PERCENT,
            SCALE_180_PERCENT,
            SCALE_200_PERCENT,
            SCALE_225_PERCENT,
            SCALE_250_PERCENT,
            SCALE_300_PERCENT,
            SCALE_350_PERCENT,
            SCALE_400_PERCENT,
            SCALE_450_PERCENT,
            SCALE_500_PERCENT
*/        };

    }
}
