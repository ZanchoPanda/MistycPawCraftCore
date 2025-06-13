using MistycPawCraftCore.DDBB;
using MistycPawCraftCore.Properties;
using MistycPawCraftCore.Utils.Language;
using System;
using System.IO;
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

            // Idioma predeterminado
            string savedLang = Settings.Default.Language ?? "es";
            LocalizationManager.ChangeLanguage(savedLang);

            // BasePath (si no se definió o no existe, usar base de aplicación)
            string basePath = ValidarRuta(Settings.Default.ImageBasePath, AppDomain.CurrentDomain.BaseDirectory);
            Settings.Default.ImageBasePath = AsegurarSlashFinal(basePath);

            // Definir rutas
            Settings.Default.ImageSetPath = ValidarRuta(Settings.Default.ImageSetPath, Path.Combine(basePath, "Sets"));
            Settings.Default.ImageCardPath = ValidarRuta(Settings.Default.ImageCardPath, Path.Combine(basePath, "Cards"));
            Settings.Default.ImageSymbolPath = ValidarRuta(Settings.Default.ImageSymbolPath, Path.Combine(basePath, "Symbol"));

            // Crear directorios
            CrearDirectorioSiNoExiste(Settings.Default.ImageSetPath);
            CrearDirectorioSiNoExiste(Settings.Default.ImageCardPath);
            CrearDirectorioSiNoExiste(Settings.Default.ImageSymbolPath);

            Settings.Default.Save();

        }

        /// <summary>
        /// Si la ruta es nula, vacía o inaccesible (por ejemplo, unidad de red desconectada), devuelve una alternativa local.
        /// </summary>
        private static string ValidarRuta(string rutaOriginal, string rutaAlternativa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rutaOriginal) || !PathIsAccessible(rutaOriginal))
                    return rutaAlternativa;

                return rutaOriginal;
            }
            catch
            {
                return rutaAlternativa;
            }
        }

        /// <summary>
        /// Asegura que el path termina con '\' o el separador de directorios correspondiente.
        /// </summary>
        private static string AsegurarSlashFinal(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                return path + Path.DirectorySeparatorChar;
            return path;
        }

        /// <summary>
        /// Intenta comprobar si el path es accesible físicamente (por ejemplo, si la unidad de red existe).
        /// </summary>
        private static bool PathIsAccessible(string path)
        {
            try
            {
                string root = Path.GetPathRoot(path);
                return !string.IsNullOrWhiteSpace(root) && Directory.Exists(root);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Crea el directorio si no existe.
        /// </summary>
        private static void CrearDirectorioSiNoExiste(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo crear el directorio:\n{path}\n\n{ex.Message}",
                                "Error al crear carpeta",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

    }

}
