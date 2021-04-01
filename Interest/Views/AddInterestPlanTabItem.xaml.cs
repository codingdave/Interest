using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Interest.Views
{
    /// <summary>
    /// Interaction logic for AddInterestPlanTabItem.xaml
    /// </summary>
    public partial class AddInterestPlanTabItem : TabItem
    {
        public AddInterestPlanTabItem()
        {
            InitializeComponent();
        }

        public ICommand AddInterestPlanCommand
        {
            get { return (ICommand)GetValue(AddInterestPlanCommandProperty); }
            set { SetValue(AddInterestPlanCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddInterestPlanCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddInterestPlanCommandProperty =
            DependencyProperty.Register("AddInterestPlanCommand", typeof(ICommand), typeof(AddInterestPlanTabItem), new PropertyMetadata(0));

    }
}
