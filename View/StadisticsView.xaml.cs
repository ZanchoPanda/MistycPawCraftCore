using LiveCharts;
using MistycPawCraftCore.DTO;
using MistycPawCraftCore.ViewModel;
using System.Windows.Controls;

namespace MistycPawCraftCore.View
{
    /// <summary>
    /// Lógica de interacción para StadisticsView.xaml
    /// </summary>
    public partial class StadisticsView : UserControl
    {
        public StadisticsView()
        {
            InitializeComponent();
        }

        private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            try
            {

                var vm = (StadisticViewModel)this.DataContext;
                if (vm != null)
                {
                    vm.SelectPie(chartPoint);
                }
                PieChartControl.Update(true, true);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void ShowCardImage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (sender is DataGrid dataGrid && dataGrid.SelectedItem is CardDto selectedItem)
                {
                    // Obtén el ViewModel del DataContext
                    StadisticViewModel viewModel = DataContext as StadisticViewModel;

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
