using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace DigitTrimmer
{
    public class TextConverter : IMultiValueConverter
    {
        private Regex _doubleRegex = new Regex(@"(-?\d+(?:\.\d+)?(?:E-?\d+)?)");

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)values[0];
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            int digits = string.IsNullOrEmpty((string) values[1])
                             ? 0
                             : int.Parse((string) values[1], CultureInfo.InvariantCulture);
            string format = digits == 0
                                ? "0"
                                : "0." + new string('#', digits);

            double scale = string.IsNullOrEmpty((string)values[2])
                               ? 1
                               : double.Parse((string)values[2], CultureInfo.InvariantCulture);

            string replace = _doubleRegex.Replace(s, m =>
                {
                    double d = double.Parse(m.Value, CultureInfo.InvariantCulture);
                    d *= scale;
                    return d.ToString(format,CultureInfo.InvariantCulture);
                });
            return replace;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
