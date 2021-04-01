using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Interest.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected virtual bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            var ret = false;
            if (!ReferenceEquals(backingField, value))
            {
                backingField = value;
                RaisePropertyChanged(propertyName);
                ret = true;
            }
            return ret;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}