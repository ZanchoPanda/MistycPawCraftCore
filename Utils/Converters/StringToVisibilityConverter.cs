using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MistycPawCraftCore.Utils.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null || value == string.Empty) return Visibility.Collapsed;

                return Visibility.Visible;
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
