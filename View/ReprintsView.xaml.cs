using MistycPawCraftCore.Utils.Language;
using MistycPawCraftCore.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MistycPawCraftCore.View
{
    /// <summary>
    /// Lógica de interacción para ReprintsView.xaml
    /// </summary>
    public partial class ReprintsView : Window
    {
        public ReprintsView()
        {
            InitializeComponent();
            LocalizationManager.LanguageChanged += (_, __) =>
            {
                Title = (string)Application.Current.TryFindResource("TituloPantallaReprint") ?? "Reprints";
            };
        }

        private void cardmarket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string UriButton = ((Button)sender).Tag.ToString();
                if (string.IsNullOrWhiteSpace(UriButton))
                {
                    return;
                }
                else
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = UriButton,
                        UseShellExecute = true
                    });
                    //Process.Start(UriButton);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TCG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string UriButton = ((Button)sender).Tag?.ToString();
                if (string.IsNullOrWhiteSpace(UriButton))
                {
                    return;
                }
                else
                {
                    //Process.Start(UriButton);
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = UriButton,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Scroll

        private void ListaBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset + e.ViewportHeight >= e.ExtentHeight - 10) // cerca del final
            {
                if (DataContext is ReprintsViewModel vm)
                {
                    vm.LoadMoreAsync();
                }
            }
        }

        #endregion

        private void Descargar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string UriButton = ((Button)sender).Tag?.ToString();
                if (string.IsNullOrWhiteSpace(UriButton))
                {
                    return;
                }
                else
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = UriButton,
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
