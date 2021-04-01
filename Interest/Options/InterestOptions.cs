using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interest.Options
{
    public class InterestOptions
    {
        public const string Options = "Options";

        public InterestOptions()
        {
            InterestPlans = new List<InterestPlanViewModelOptions>();
        }

        public List<InterestPlanViewModelOptions> InterestPlans { get; }
    }
}
