using MistycPawCraftCore.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace MistycPawCraftCore.View
{
    /// <summary>
    /// Lógica de interacción para ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        public ImportWindow()
        {
            InitializeComponent();

            if (DataContext != null)
            {
                ((ImportViewModel)DataContext).RequestClose += (sender, e) => Close();
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
