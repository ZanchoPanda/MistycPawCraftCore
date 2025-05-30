using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MistycPawCraftCore.Utils.Enums
{
    public class EnumToLocalizedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            string key = $"{value.GetType().Name}_{value}";
            return Application.Current.TryFindResource(key) ?? value.ToString(); // fallback
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Solo necesario si haces selección inversa por texto
            throw new NotImplementedException();
        }
    }
}
