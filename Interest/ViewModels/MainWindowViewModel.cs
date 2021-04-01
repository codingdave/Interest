using Interest.Commands;
using Interest.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;

namespace Interest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, ICreateWindow, IOnClose, IShowMessage
    {
        public MainWindowViewModel(IConfiguration configuration)
        {
            var options = configuration.Get<Rootobject>() ?? new Rootobject();
            InitiateOptions(options);

            InterestPlanViewModels = new ObservableCollection<InterestPlanViewModel>(options.InterestPlanViewModelOptions.Select(ip => new InterestPlanViewModel(ip)));
            InterestPlanViewModels.CollectionChanged += InterestPlanViewModels_CollectionChanged;
            SelectedCulture = CultureInfo.GetCultureInfo(options.CultureInfo);
            Cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            CreateWindowCommand = new DelegateCommand(() => CreateWindow?.Invoke());

            AddInterestPlanCommand = new DelegateCommand(() =>
            {
                var p = new InterestPlanViewModel(InterestPlanViewModelOption.GetExample1());
                InterestPlanViewModels.Add(p);
                SelectedInterestPlanViewModelIndex = InterestPlanViewModels.IndexOf(p);
            });

            DeleteInterestPlanCommand = new DelegateCommand(() => InterestPlanViewModels.Remove(InterestPlanViewModels.ElementAt(SelectedInterestPlanViewModelIndex)));

            ResetAllCommand = new DelegateCommand(() => InterestPlanViewModels.ToList().ForEach(ip => ip.ResetCommand.Execute()));

            CalculateAllCommand = new DelegateCommand(() => InterestPlanViewModels.ToList().ForEach(ip => ip.CalculateCommand.Execute()));

            ResetAllCommand.Execute();
        }

        private static void InitiateOptions(Rootobject options)
        {
            if (options.InterestPlanViewModelOptions.Count == 0)
            {
                options.InterestPlanViewModelOptions.Add(InterestPlanViewModelOption.GetExample1());
            }
            if (options.CultureInfo == null)
            {
                options.CultureInfo = CultureInfo.InvariantCulture.ToString();
            }
        }

        private void InterestPlanViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(InterestPlanViewModels));
            RaisePropertyChanged(nameof(TotalInterest));
            RaisePropertyChanged(nameof(ResidualDebt));
        }

        public double TotalInterest => InterestPlanViewModels.Count == 0 ? 0 : InterestPlanViewModels.Select(a => a.TotalInterest).Aggregate((a, b) => a + b);

        public double ResidualDebt => InterestPlanViewModels.Count == 0 ? 0 : InterestPlanViewModels.Select(a => a.ResidualDebt).Aggregate((a, b) => a + b);

        public CultureInfo SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                if (value != _selectedCulture)
                {
                    var old = _selectedCulture;
                    _selectedCulture = value;
                    CultureInfo.CurrentCulture = value;
                    ShowMessage?.Invoke($"Changing the culture from '{old}' to '{value}'. A restart is necessary to apply these changes.");
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
                options.InterestPlanViewModelOptions.Add(m.Values);
            }
            options.CultureInfo = CultureInfo.CurrentCulture.ToString();

            var joptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize(options, joptions);

            File.WriteAllText("appsettings.json", json);
        }
    }
}
