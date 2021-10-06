using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ItaCursor.Setting.ViewModel
{
    /// <summary>
    /// AndroidのLiveDataみたいなやつ？INotifyPropertyChangedを使えば ViewModel -> View の通知が飛ばせるらしい。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ValueChanged<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
         => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private T _value;

        public T value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(_value));
            }
        }

        public ValueChanged(T value)
        {
            _value = value;
        }

    }
}
