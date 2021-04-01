using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Interest.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        private static readonly StringFormatConverter instance = new StringFormatConverter();
        public static StringFormatConverter Instance
        {
            get
            {
                return instance;
            }
        }

        private StringFormatConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(culture, value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
