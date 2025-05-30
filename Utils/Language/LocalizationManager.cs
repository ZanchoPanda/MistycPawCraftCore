using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace MistycPawCraftCore.Utils.Language
{
    public class LocalizationManager
    {

        public static event EventHandler LanguageChanged;

        public static void ChangeLanguage(string cultureCode)
        {
            try
            {
                var cultureInfo = new CultureInfo(cultureCode);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

                var oldDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Dictionary."));
                if (oldDict != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                }

                ResourceDictionary dict = new ResourceDictionary
                {
                    Source = new Uri($"/MistycPawCraftCore;component/Utils/Language/Dictionary.{cultureCode}.xaml", UriKind.Relative)
                };

                Application.Current.Resources.MergedDictionaries.Add(dict);

                LanguageChanged?.Invoke(Application.Current, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                var aux = ex;
                throw ex;
            }
        }

        public static string SearchValue(string KeyString)
        {
            try
            {
                CultureInfo currentCultu = CultureInfo.CurrentCulture;

                ResourceDictionary dict = new ResourceDictionary
                {
                    Source = new Uri($"/MistycPawCraftCore;component/Utils/Language/Dictionary.{currentCultu.Name}.xaml", UriKind.Relative)
                };

                if (dict.Contains(KeyString))
                {
                    return dict[KeyString].ToString();
                }

                return "___";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetString(string key)
        {
            var resource = Application.Current.TryFindResource(key);
            return resource?.ToString() ?? $"[[{key}]]";
        }

    }
}
