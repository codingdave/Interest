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
            InterestPlanViewModels.CollectionChanged += InterestPlanViewModels_CollectionChanged;

            CreateWindowCommand = new DelegateCommand(() => CreateWindow?.Invoke());

            AddInterestPlanCommand = new DelegateCommand(() =>
            {
                var p = new InterestPlanViewModel(InterestPlanViewModelOptions.GetDefault());
                InterestPlanViewModels.Add(p);
                SelectedInterestPlanViewModelIndex = InterestPlanViewModels.IndexOf(p);
            });

            DeleteInterestPlanCommand = new DelegateCommand(() => InterestPlanViewModels.Remove(InterestPlanViewModels.ElementAt(SelectedInterestPlanViewModelIndex)));

            ResetAllCommand = new DelegateCommand(() => InterestPlanViewModels.ToList().ForEach(ip => ip.ResetCommand.Execute()));

            CalculateAllCommand = new DelegateCommand(() => InterestPlanViewModels.ToList().ForEach(ip => ip.CalculateCommand.Execute()));

            ResetAllCommand.Execute();
        }

        private void InterestPlanViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(InterestPlanViewModels));
            RaisePropertyChanged(nameof(TotalInterest));
            RaisePropertyChanged(nameof(ResidualDebt));
        }

        public double TotalInterest => InterestPlanViewModels.Count == 0 ? 0 : InterestPlanViewModels.Select(a => a.TotalInterest).Aggregate((a, b) => a + b);

        public double ResidualDebt => InterestPlanViewModels.Count == 0 ? 0 : InterestPlanViewModels.Select(a => a.ResidualDebt).Aggregate((a, b) => a + b);

        public ICommand CreateWindowCommand { get; private set; }
        public Action CreateWindow { get; set; }

        public ObservableCollection<InterestPlanViewModel> InterestPlanViewModels { get; }

        public ICommand AddInterestPlanCommand { get; set; }
        public DelegateCommand DeleteInterestPlanCommand { get; }
        public DelegateCommand ResetAllCommand { get; }
        public DelegateCommand CalculateAllCommand { get; }

        private int _selectedInterestPlanViewModelIndex;

        public int SelectedInterestPlanViewModelIndex { get => _selectedInterestPlanViewModelIndex; set => SetProperty(ref _selectedInterestPlanViewModelIndex, value); }

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
