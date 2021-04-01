using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Interest
{

    class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = string.Concat(((double)value).ToString() + "%");
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = ((string)value).AsSpan();
            if (s[s.Length - 1] == '%')
            {
                s = s.Slice(0, s.Length - 1);
            }
            double.TryParse(s.ToString(), out var res);
            return res;
        }
    }
}
