using System;
using System.Windows.Input;

namespace ItaCursor.Setting.ViewModel
{
    class ViewModelButtonClick : ICommand
    {
        private Action click;

        /// <summary>
        /// ViewModelでクリックイベントをバインディングするときにつかう便利なやつ？。以下例
        /// 
        /// <code>
        /// 
        /// public ViewModelButtonClick OpenGitHub = new ViewModelButtonClick(() =>
        /// {
        ///     ProcessStartInfo psi = new ProcessStartInfo
        ///     {
        ///         FileName = "https://github.com/takusan23",
        ///         UseShellExecute = true
        ///     };
        ///     Process.Start(psi);
        /// });
        /// 
        /// </code>
        /// 
        /// </summary>
        public ViewModelButtonClick(Action click)
        {
            this.click = click;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            click();
        }
    }
}
