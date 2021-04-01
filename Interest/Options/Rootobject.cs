using System.Collections.Generic;

namespace Interest.Options
{
    public class Rootobject
    {
        public List<InterestPlanViewModelOption> InterestPlanViewModelOptions { get; set; } = new List<InterestPlanViewModelOption>();
        public Logging Logging { get; set; } = new Logging();
    }
}