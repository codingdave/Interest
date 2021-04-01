using System;
using System.Globalization;
using System.Windows.Data;

namespace Interest
{
    class UnscheduledRepaymentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var o = (UnscheduledRepayment)value;
            var ret = o.Value;
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse((string)value, out var val))
            {
                return new UnscheduledRepayment(val, InputType.Manual);
            }
            else
            {
                return new UnscheduledRepayment(0, InputType.Auto);
            }
        }
    }
}
