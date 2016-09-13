namespace DigitTrimmer
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    public class TextConverter : IMultiValueConverter
    {
        public static readonly TextConverter Default = new TextConverter();

        private static readonly Regex DoubleRegex = new Regex(@"(-?\d+(?:\.\d+)?(?:E-?\d+)?)");

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)values[0];
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var digits = string.IsNullOrEmpty((string)values[1])
                             ? 0
                             : int.Parse((string)values[1], CultureInfo.InvariantCulture);
            var scale = string.IsNullOrEmpty((string)values[2])
                   ? 1
                   : double.Parse((string)values[2], CultureInfo.InvariantCulture);
            return Convert(text, digits, scale);
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static string Convert(string text, int digits, double scale)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var format = digits == 0
                                ? "0"
                                : $"0.{new string('#', digits)}";

            var replace = DoubleRegex.Replace(text, m =>
            {
                var d = double.Parse(m.Value, CultureInfo.InvariantCulture);
                d *= scale;
                return d.ToString(format, CultureInfo.InvariantCulture);
            });
            return replace;
        }
    }
}
