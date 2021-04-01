using Interest.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Interest.ViewModels
{
    public interface IMainWindowViewModel
    {
        ICommand AddInterestPlanCommand { get; set; }
        DelegateCommand CalculateAllCommand { get; }
        ICommand CreateWindowCommand { get; }
        ObservableCollection<InterestPlanViewModel> InterestPlanViewModels { get; }
        DelegateCommand ResetAllCommand { get; }
        double ResidualDebt { get; }
        double TotalInterest { get; }
    }
}