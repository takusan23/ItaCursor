using Microsoft.Win32;
using System.Windows.Media;

namespace ItaCursor.Tool
{
    class WindowsAccentColor
    {

        /// <summary>
        /// Windows10のアクセントカラーを取得する。レジストリ経由で
        /// </summary>
        /// <returns>失敗したらnull</returns>
        public static Color? GetAccentColor()
        {
            var DWM_KEY = @"Software\Microsoft\Windows\DWM";
            var dwmKey = Registry.CurrentUser.OpenSubKey(DWM_KEY, RegistryKeyPermissionCheck.ReadSubTree);

            object accentColorObj = dwmKey.GetValue("AccentColor");
            if (accentColorObj is int accentColorDword)
            {
                var alpha = (accentColorDword >> 24) & 0xFF;
                var blue = (accentColorDword >> 16) & 0xFF;
                var green = (accentColorDword >> 8) & 0xFF;
                var red = (accentColorDword >> 0) & 0xFF;
                return Color.FromArgb((byte)alpha, (byte)red, (byte)green, (byte)blue);
            }
            return null;
        }

    }
}
