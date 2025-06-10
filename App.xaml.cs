using MistycPawCraftCore.DDBB;
using MistycPawCraftCore.Properties;
using MistycPawCraftCore.Utils.Language;
using System;
using System.Windows;
using Application = System.Windows.Application;

namespace MistycPawCraftCore
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Resources["IsDebug"] = IsDebug;
            DatabaseHelper.InitializeDatabase();

            string savedLang = Settings.Default.Language ?? "es";
            LocalizationManager.ChangeLanguage(savedLang);

            Settings.Default.ImageBasePath = string.IsNullOrWhiteSpace(Settings.Default.ImageBasePath) ? AppDomain.CurrentDomain.BaseDirectory : Settings.Default.ImageBasePath;
            Settings.Default.ImageSetPath = string.IsNullOrWhiteSpace(Settings.Default.ImageSetPath) ? $"{Settings.Default.ImageBasePath}Sets\\" : Settings.Default.ImageSetPath;
            Settings.Default.ImageCardPath = string.IsNullOrWhiteSpace(Settings.Default.ImageCardPath) ? $"{Settings.Default.ImageBasePath}Cards\\" : Settings.Default.ImageCardPath;
            Settings.Default.ImageSymbolPath = string.IsNullOrWhiteSpace(Settings.Default.ImageSymbolPath) ? $"{Settings.Default.ImageBasePath}Symbol\\" : Settings.Default.ImageSymbolPath;

            Settings.Default.Save();

        }

    }

}
