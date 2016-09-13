namespace DigitTrimmer
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    public class TextConverter : IMultiValueConverter
    {
        private static readonly Regex DoubleRegex = new Regex(@"(-?\d+(?:\.\d+)?(?:E-?\d+)?)");

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

            string replace = DoubleRegex.Replace(s, m =>
                {
                    double d = double.Parse(m.Value, CultureInfo.InvariantCulture);
                    d *= scale;
                    return d.ToString(format,CultureInfo.InvariantCulture);
                });
            return replace;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
