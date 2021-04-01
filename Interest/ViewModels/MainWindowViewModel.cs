using Interest.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Interest.ViewModels
{
    public class MainWindowViewModel : BindableBase, ICreateWindow
    {
        public MainWindowViewModel()
        {
            CreateWindowCommand = new DelegateCommand(() => CreateWindow?.Invoke());
        }
        public ICommand CreateWindowCommand { get; private set; }
        public Action CreateWindow { get; set; }
    }
}
