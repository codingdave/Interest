using Interest.Types;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Interest.Converters
{
    class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object ret = value;
            if (value is Percentage p)
            {
                switch (parameter)
                {
                    case DateKind.Year:
                        ret = string.Concat(p.PerYear.ToString() + "%");
                        break;

                    case DateKind.Month:
                        ret = string.Concat(p.PerMonth.ToString() + "%");
                        break;
                    default: throw new InvalidOperationException("need parameter Month or Year");
                }
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = value;
            var s = ((string)value).AsSpan();
            if (s[^1] == '%')
            {
                s = s[0..^1];
            }
            if (double.TryParse(s.ToString(), out var res))
            {
                switch (parameter)
                {
                    case DateKind.Year:
                        ret = new Percentage(res);
                        break;

                    case DateKind.Month:
                        ret = new Percentage(res * 12);
                        break;
                    default: throw new InvalidOperationException("need parameter Month or Year");
                }
            }
            return ret;
        }
    }
}
