using Interest.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Interest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, ICreateWindow
    {
        public MainWindowViewModel(ConfigurationManagerReaderWriter reader)
        {
            InterestPlanViewModels = new ObservableCollection<InterestPlanViewModel>() { new InterestPlanViewModel(reader, "first"), };

            CreateWindowCommand = new DelegateCommand(() => CreateWindow?.Invoke());
            AddInterestPlanCommand = new DelegateCommand(() => InterestPlanViewModels.Add(new InterestPlanViewModel(reader, "...")));

            ResetAllCommand = new DelegateCommand(() => InterestPlanViewModels.ToList().ForEach(ip => ip.ResetCommand.Execute()));

            CalculateAllCommand = new DelegateCommand(() =>
            {
                InterestPlanViewModels.ToList().ForEach(ip => ip.CalculateCommand.Execute());
                RaisePropertyChanged(nameof(TotalInterest));
                RaisePropertyChanged(nameof(ResidualDebt));
            });
        }

        public double TotalInterest => InterestPlanViewModels.Select(a => a.TotalInterest).Aggregate((a, b) => a + b);

        public double ResidualDebt => InterestPlanViewModels.Select(a => a.ResidualDebt).Aggregate((a, b) => a + b);

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
        public DelegateCommand ResetAllCommand { get; }
        public DelegateCommand CalculateAllCommand { get; }
    }
}
