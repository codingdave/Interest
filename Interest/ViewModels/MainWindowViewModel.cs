using Interest.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            InterestPlanViewModels = new ObservableCollection<InterestPlanViewModel>() { new InterestPlanViewModel("first"), };

            CreateWindowCommand = new DelegateCommand(() => CreateWindow?.Invoke());
            AddInterestPlanCommand = new DelegateCommand(() => InterestPlanViewModels.Add(new InterestPlanViewModel("...")));
        }
        public ICommand CreateWindowCommand { get; private set; }
        public Action CreateWindow { get; set; }

        #region InterestPlanViewModels
        private ObservableCollection<InterestPlanViewModel> _interestPlanViewModels;
        public ObservableCollection<InterestPlanViewModel> InterestPlanViewModels
        {
            get => _interestPlanViewModels;
            set => _ = SetProperty(ref _interestPlanViewModels, value);
        } 
        #endregion

        public ICommand AddInterestPlanCommand { get; set; }
    }
}
