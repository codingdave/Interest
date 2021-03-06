using Interest.Options;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Interest.ViewModels
{
    public class MainWindowViewModelDesignTime : MainWindowViewModel
    {
        public MainWindowViewModelDesignTime() : base(new ConfigurationBuilder().Build())
        {
            InterestPlanViewModels.Clear();
            InterestPlanViewModels.Add(new InterestPlanViewModel(InterestPlanViewModelOption.GetExample1()));
            InterestPlanViewModels.Add(new InterestPlanViewModel(InterestPlanViewModelOption.GetExample2()));
            Cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        }       
    }
}
