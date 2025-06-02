using System;
using System.Globalization;
using System.Windows.Data;

namespace MistycPawCraftCore.Utils.Converters
{
    public class StringToUriConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            if (string.IsNullOrWhiteSpace(path)) return null;
            return new Uri(path, UriKind.RelativeOrAbsolute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

    }
}
