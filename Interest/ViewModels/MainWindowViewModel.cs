using Interest.Commands;
using Interest.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Interest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, ICreateWindow, IMainWindowViewModel
    {
        public MainWindowViewModel()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                throw new InvalidOperationException("Design time only");
            }

            var options = new InterestOptions();
            options.InterestPlans.Add(new InterestPlanViewModelOptions() { Title = "First" });
            options.InterestPlans.Add(new InterestPlanViewModelOptions() { Title = "Second" });
            InterestPlanViewModels = new ObservableCollection<InterestPlanViewModel>(options.InterestPlans.Select(ip => new InterestPlanViewModel(ip)));
        }

        public MainWindowViewModel(IConfiguration configuration)
        {
            var options = configuration.Get<InterestOptions>();
            if (options == null || options.InterestPlans.Count == 0)
            {
                options = new InterestOptions();
                options.InterestPlans.Add(new InterestPlanViewModelOptions());
            }

            InterestPlanViewModels = new ObservableCollection<InterestPlanViewModel>(options.InterestPlans.Select(ip => new InterestPlanViewModel(ip)));

            CreateWindowCommand = new DelegateCommand(() => CreateWindow?.Invoke());
            AddInterestPlanCommand = new DelegateCommand(() => InterestPlanViewModels.Add(new InterestPlanViewModel(new InterestPlanViewModelOptions())));

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
            set
            {
                if (SetProperty(ref _interestPlanViewModels, value))
                {
                    SelectedInterestPlanViewModel = value[0];
                }
            }
        }
        #endregion

        public ICommand AddInterestPlanCommand { get; set; }
        public DelegateCommand ResetAllCommand { get; }
        public DelegateCommand CalculateAllCommand { get; }

        private InterestPlanViewModel _selectedInterestPlanViewModel;

        public InterestPlanViewModel SelectedInterestPlanViewModel { get => _selectedInterestPlanViewModel; set => SetProperty(ref _selectedInterestPlanViewModel, value); }
    }
}
