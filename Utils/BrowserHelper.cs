using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;

namespace MistycPawCraftCore.Utils
{
    public class BrowserHelper
    {

        public static string GetDefaultBrowserPath()
        {
            const string browserKey = @"HTTP\shell\open\command";
            string defaultBrowserPath = null;

            try
            {
                using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(browserKey))
                {
                    if (registryKey != null)
                    {
                        // Get the (default) value, which contains the path to the executable
                        string rawPath = registryKey.GetValue(null) as string;

                        if (!string.IsNullOrEmpty(rawPath))
                        {
                            // The value might have extra parameters, so we extract the executable path
                            defaultBrowserPath = rawPath.Trim('"').Split(' ')[2];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., security exceptions, if necessary
                Console.WriteLine($"Error retrieving default browser path: {ex.Message}");
            }

            return defaultBrowserPath;
        }

        /// <summary>
        /// Launch URL in private mode with default browser
        /// In case not controlled, it fires in IeExplorer
        /// </summary>
        /// <param name="browserName"></param>
        /// <param name="urlFromDb"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void LaunchUrl(string browserName, string urlFromDb)
        {
            try
            {
                string privateModeParam = string.Empty;

                if (string.IsNullOrEmpty(browserName))
                {
                    MessageBox.Show("No default browser found!");
                }
                else
                {
                    if (browserName.Contains("firefox"))
                    {
                        browserName = "firefox.exe";
                        privateModeParam = " -private-window";
                    }
                    else if (browserName.Contains("iexplore"))
                    {
                        browserName = "iexplore.exe";
                        privateModeParam = " -private";
                    }
                    else if (browserName.Contains("chrome"))
                    {
                        browserName = "chrome.exe";
                        privateModeParam = " -incognito";
                    }
                    else if (browserName.Contains("Opera"))
                    {
                        browserName = "opera.exe";
                        privateModeParam = " -private";
                    }
                    else
                    {
                        browserName = "iexplore.exe";
                        privateModeParam = " -private";
                    }

                    //Process.Start(browserName, $"{privateModeParam} {urlFromDb}");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "browserName",
                        Arguments = $"{privateModeParam} {urlFromDb}",
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
