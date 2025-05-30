using MistycPawCraftCore.DDBB;
using MistycPawCraftCore.Properties;
using MistycPawCraftCore.Utils.Language;
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
        }

    }

}
