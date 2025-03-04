using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ByteCompare
{
    public class BooleanToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Application.Current.Resources["DifferenceHighlightBrush"] : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}