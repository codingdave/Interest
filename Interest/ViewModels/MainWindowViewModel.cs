using Interest.Commands;
using Interest.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows.Input;

namespace Interest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, ICreateWindow, IOnClose, IShowMessage
    {
        public MainWindowViewModel(IConfiguration configuration)
        {
            InterestPlanViewModels = new ObservableCollection<InterestPlanViewModel>();
            InterestPlanViewModels.CollectionChanged += InterestPlanViewModels_CollectionChanged;

            var options = configuration.Get<Rootobject>() ?? new Rootobject();
            InitiateOptions(options);
            options.InterestPlanViewModelOptions.ForEach(ip => AddInterestPlanViewModel(new InterestPlanViewModel(ip)));

            SelectedCulture = CultureInfo.GetCultureInfo(options.CultureInfo);
            Cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            CreateWindowCommand = new DelegateCommand(() => CreateWindow?.Invoke());

            AddInterestPlanCommand = new DelegateCommand(() =>
            {
                var p = new InterestPlanViewModel(InterestPlanViewModelOption.GetExample1());
                AddInterestPlanViewModel(p);
                SelectedInterestPlanViewModelIndex = InterestPlanViewModels.IndexOf(p);
            });

            DeleteInterestPlanCommand = new DelegateCommand(
                () =>
                {
                    var interestPlanViewModel = InterestPlanViewModels.ElementAt(SelectedInterestPlanViewModelIndex);
                    interestPlanViewModel.PropertyChanged -= InterestPlanViewModel_PropertyChanged;
                    InterestPlanViewModels.Remove(interestPlanViewModel);
                },
                () => InterestPlanViewModels.Count > 1
                );

            ResetAllCommand = new DelegateCommand(() => InterestPlanViewModels.ToList().ForEach(ip => ip.ResetCommand.Execute()));

            CalculateAllCommand = new DelegateCommand(() => InterestPlanViewModels.ToList().ForEach(ip => ip.CalculateCommand.Execute()));

            ResetAllCommand.Execute();
        }

        private void InterestPlanViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateOverallTotalInterest();
            UpdateOverallResidualDebt();
        }

        private void AddInterestPlanViewModel(InterestPlanViewModel interestPlanViewModel)
        {
            interestPlanViewModel.PropertyChanged += InterestPlanViewModel_PropertyChanged;
            InterestPlanViewModels.Add(interestPlanViewModel);
        }

        private void UpdateOverallResidualDebt()
        {
            OverallResidualDebt = InterestPlanViewModels.Count == 0 ? 0 : InterestPlanViewModels.Select(a => a.ResidualDebt).Aggregate((a, b) => a + b);
        }

        private void UpdateOverallTotalInterest()
        {
            OverallTotalInterest = InterestPlanViewModels.Count == 0 ? 0 : InterestPlanViewModels.Select(a => a.TotalInterest).Aggregate((a, b) => a + b);
        }

        private void InterestPlanViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(InterestPlanViewModel.TotalInterest):
                    UpdateOverallTotalInterest();
                    break;

                case nameof(InterestPlanViewModel.ResidualDebt):
                    UpdateOverallResidualDebt();
                    break;
            }
        }

        private static void InitiateOptions(Rootobject options)
        {
            if (options.InterestPlanViewModelOptions.Count == 0)
            {
                options.InterestPlanViewModelOptions.Add(InterestPlanViewModelOption.GetExample1());
            }
            if (options.CultureInfo == null)
            {
                options.CultureInfo = Thread.CurrentThread.CurrentUICulture.ToString();
            }
        }

        #region OverallTotalInterest
        private double _overallTotalInterest;
        public double OverallTotalInterest
        {
            get => _overallTotalInterest;
            set => SetProperty(ref _overallTotalInterest, value);
        }
        #endregion

        #region OverallResidualDebt
        private double _overallResidualDebt;
        public double OverallResidualDebt
        {
            get => _overallResidualDebt;
            set => SetProperty(ref _overallResidualDebt, value);
        }
        #endregion

        public CultureInfo SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                if (value != _selectedCulture)
                {
                    var old = _selectedCulture;
                    ShowMessage?.Invoke($"Changing the culture from '{old}' to '{value}'. A restart is necessary to apply these changes.");

                    _selectedCulture = value;
                    CultureInfo.CurrentCulture = value;
                    Thread.CurrentThread.CurrentUICulture = value;
                }
            }
        }

        public CultureInfo[] Cultures { get; protected set; }
        public ICommand CreateWindowCommand { get; private set; }
        public Action CreateWindow { get; set; }
        public ObservableCollection<InterestPlanViewModel> InterestPlanViewModels { get; }
        public ICommand AddInterestPlanCommand { get; set; }
        public DelegateCommand DeleteInterestPlanCommand { get; }
        public DelegateCommand ResetAllCommand { get; }
        public DelegateCommand CalculateAllCommand { get; }

        private int _selectedInterestPlanViewModelIndex;
        private static CultureInfo _selectedCulture;

        public int SelectedInterestPlanViewModelIndex { get => _selectedInterestPlanViewModelIndex; set => SetProperty(ref _selectedInterestPlanViewModelIndex, value); }
        public Action<string> ShowMessage { get; set; }

        public void OnClose()
        {
            SaveAppsettings();
        }

        private void SaveAppsettings()
        {
            var options = new Rootobject();
            foreach (var m in InterestPlanViewModels)
            {
                options.InterestPlanViewModelOptions.Add(m.Options);
            }

            options.CultureInfo = SelectedCulture.ToString();

            var joptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize(options, joptions);

            File.WriteAllText("appsettings.json", json);
        }
    }
}
