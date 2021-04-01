using Interest.Options;
using Microsoft.Extensions.Configuration;

namespace Interest.ViewModels
{
    public class MainWindowViewModelDesignTime : MainWindowViewModel
    {
        public MainWindowViewModelDesignTime() : base(new ConfigurationBuilder().Build())
        {
            InterestPlanViewModels.Clear();
            InterestPlanViewModels.Add(new InterestPlanViewModel(InterestPlanViewModelOptions.GetDefault()));
            InterestPlanViewModels.Add(new InterestPlanViewModel(InterestPlanViewModelOptions.GetDefault2()));
        }
    }
}
