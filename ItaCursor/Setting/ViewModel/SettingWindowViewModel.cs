using System;
using System.Windows.Input;

namespace ItaCursor.Setting.ViewModel
{
    class SettingWindowViewModel
    {
        public ValueChanged<string> NavigationPath { get; set; } = new ValueChanged<string>("SettingPage/FirstSettingPage.xaml");

        public ICommand ToFirstSettingScreen { get; set; }

        public ICommand ToAppInfoSettingScreen { get; set; }

        public SettingWindowViewModel()
        {
            ToFirstSettingScreen = new ViewModelButtonClick(() =>
            {
                NavigationPath.value = "SettingPage/FirstSettingPage.xaml";
            });

            ToAppInfoSettingScreen = new ViewModelButtonClick(() =>
            {
                NavigationPath.value = "SettingPage/AppInfoSettingPage.xaml";
            });
        }

    }

    class NavigationEventArgs : EventArgs
    {
        private string _path;
        public string path { get => _path; }

        public NavigationEventArgs(string path)
        {
            this._path = path;
        }
    }

}
