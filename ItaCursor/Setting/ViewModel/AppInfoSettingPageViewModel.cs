using System.Diagnostics;
using System.Reflection;

namespace ItaCursor.Setting.ViewModel
{
    class AppInfoSettingPageViewModel
    {

        public string AppName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        public string Version { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// ソースコードボタンを押したとき
        /// </summary>
        public ViewModelButtonClick OpenGitHub { get; set; } = new ViewModelButtonClick(() =>
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://github.com/takusan23/ItaCursor",
                UseShellExecute = true
            };
            Process.Start(psi);
        });


    }
}
