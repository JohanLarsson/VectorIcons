namespace VectorIcons.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class MultiplyConverter : IValueConverter
    {
        public static readonly MultiplyConverter Default = new MultiplyConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = System.Convert.ToDouble(value);
            var p = System.Convert.ToDouble(parameter);
            return d * p;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
