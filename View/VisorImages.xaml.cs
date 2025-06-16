using MistycPawCraftCore.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace MistycPawCraftCore.View
{
    /// <summary>
    /// Lógica de interacción para VisorImages.xaml
    /// </summary>
    public partial class VisorImages : Window
    {
        public VisorImages()
        {
            InitializeComponent();
        }

        public VisorImages(string url)
        {
            InitializeComponent();
            try
            {
                //webBrowser.Source = new Uri(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading URL: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Card_Clicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DataContext is VisorImagesViewModel vm)
                {
                    vm.ToggleFace();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Card_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is VisorImagesViewModel vm)
                {
                    vm.ToggleFace();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DataContext is VisorImagesViewModel vm)
                {
                    vm.ToggleFace();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
