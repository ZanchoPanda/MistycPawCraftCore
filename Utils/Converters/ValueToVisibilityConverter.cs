using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MistycPawCraftCore.Utils.Converters
{
    public class ValueToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return Visibility.Collapsed;

                double cmc = System.Convert.ToDouble(value);
                return cmc > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
