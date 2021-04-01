using Interest.Commands;
using Interest.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;

namespace Interest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel, ICreateWindow, IOnClose
    {
        public MainWindowViewModel()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                throw new InvalidOperationException("Design time only");
            }

            var interestPlanViewModelOptions = new List<InterestPlanViewModelOptions>();
            interestPlanViewModelOptions.Add(InterestPlanViewModelOptions.GetDefault());
            interestPlanViewModelOptions.Add(InterestPlanViewModelOptions.GetDefault2());
            InterestPlanViewModels = new ObservableCollection<InterestPlanViewModel>(interestPlanViewModelOptions.Select(ip => new InterestPlanViewModel(ip)));
        }

        public MainWindowViewModel(IConfiguration configuration)
        {
            var options = configuration.Get<Rootobject>() ?? new Rootobject();
            if (options.InterestPlanViewModelOptions.Count == 0)
            {
                options.InterestPlanViewModelOptions.Add(InterestPlanViewModelOptions.GetDefault());
            }

            InterestPlanViewModels = new ObservableCollection<InterestPlanViewModel>(options.InterestPlanViewModelOptions.Select(ip => new InterestPlanViewModel(ip)));

            CreateWindowCommand = new DelegateCommand(() => CreateWindow?.Invoke());
            AddInterestPlanCommand = new DelegateCommand(() =>
            {
                var p = new InterestPlanViewModel(InterestPlanViewModelOptions.GetDefault());
                InterestPlanViewModels.Add(p);
                SelectedInterestPlanViewModel = p;
            });
            DeleteInterestPlanCommand = new DelegateCommand(() => InterestPlanViewModels.Remove(SelectedInterestPlanViewModel));

            ResetAllCommand = new DelegateCommand(() => InterestPlanViewModels.ToList().ForEach(ip => ip.ResetCommand.Execute()));

            CalculateAllCommand = new DelegateCommand(() =>
            {
                InterestPlanViewModels.ToList().ForEach(ip => ip.CalculateCommand.Execute());
                RaisePropertyChanged(nameof(TotalInterest));
                RaisePropertyChanged(nameof(ResidualDebt));
            });

            ResetAllCommand.Execute();
        }

        public double TotalInterest => InterestPlanViewModels.Select(a => a.TotalInterest).Aggregate((a, b) => a + b);

        public double ResidualDebt => InterestPlanViewModels.Select(a => a.ResidualDebt).Aggregate((a, b) => a + b);

        public ICommand CreateWindowCommand { get; private set; }
        public Action CreateWindow { get; set; }

        public ObservableCollection<InterestPlanViewModel> InterestPlanViewModels { get; }

        public ICommand AddInterestPlanCommand { get; set; }
        public DelegateCommand DeleteInterestPlanCommand { get; }
        public DelegateCommand ResetAllCommand { get; }
        public DelegateCommand CalculateAllCommand { get; }

        private InterestPlanViewModel _selectedInterestPlanViewModel;

        public InterestPlanViewModel SelectedInterestPlanViewModel { get => _selectedInterestPlanViewModel; set => SetProperty(ref _selectedInterestPlanViewModel, value); }

        public void OnClose()
        {
            var options = new Rootobject();
            foreach (var m in InterestPlanViewModels)
            {
                options.InterestPlanViewModelOptions.Add(m.Values);
            }

            var joptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize(options, joptions);

            File.WriteAllText("appsettings.json", json);
        }
    }
}
