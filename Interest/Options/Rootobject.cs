using System.Collections.Generic;
using System.Globalization;

namespace Interest.Options
{
    public class Rootobject
    {
        public string CultureInfo { get; set; }
        public List<InterestPlanViewModelOption> InterestPlanViewModelOptions { get; set; } = new List<InterestPlanViewModelOption>();
        public Logging Logging { get; set; } = new Logging();
    }
}