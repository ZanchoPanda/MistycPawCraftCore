using MistycPawCraftCore.DTO;
using MistycPawCraftCore.ViewModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace MistycPawCraftCore.View
{
    /// <summary>
    /// Lógica de interacción para BuscadorView.xaml
    /// </summary>
    public partial class DeckView : UserControl, INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
        public DeckView()
        {
            InitializeComponent();
        }

        private void ShowCardImage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (sender is DataGrid dataGrid && dataGrid.SelectedItem is CardDto selectedItem)
                {
                    // Obtén el ViewModel del DataContext
                    DeckViewModel viewModel = DataContext as DeckViewModel;

                    // Llama al método en el ViewModel
                    viewModel?.HandleDoubleClick(selectedItem);
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
