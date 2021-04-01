using Interest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Prism.Mvvm;

namespace Interest.Views
{
    public partial class InterestPlan : UserControl
    {
        public InterestPlan()
        {
            InitializeComponent();
        }

        public bool UnscheduledRepayment
        {
            get { return (bool)GetValue(UnscheduledRepaymentProperty); }
            set { SetValue(UnscheduledRepaymentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnscheduledRepayment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnscheduledRepaymentProperty =
            DependencyProperty.Register("UnscheduledRepayment", typeof(bool), typeof(InterestPlan), new PropertyMetadata(UnscheduledRepaymentChanged));

        private static void UnscheduledRepaymentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
