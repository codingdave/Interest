using System;
using System.Globalization;
using System.Windows.Data;

namespace Interest.Converters
{
    class InputValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var o = (InputValue<double>)value;
            var ret = o.Value;
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object ret = value;
            if (double.TryParse((string)value, out var val))
            {
                ret = new InputValue<double>(val, InputType.Manual);
            }
            return ret;
        }
    }
}
