using MistycPawCraftCore.Utils;
using MistycPawCraftCore.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MistycPawCraftCore.View
{
    /// <summary>
    /// Lógica de interacción para TorneosEventos.xaml
    /// </summary>
    public partial class TorneosEventos : UserControl, INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        public TorneosEventos()
        {
            InitializeComponent();
            EventosCalendar.Loaded += EventosCalendar_Loaded;
            EventosCalendar.DisplayDateChanged += EventosCalendar_DisplayDateChanged;
        }

        private void EventosCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            ResaltarDiasConEventos();
        }

        private void EventosCalendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            ResaltarDiasConEventos();
        }

        private void ResaltarDiasConEventos()
        {
            try
            {

                if (DataContext is TorneosEventosViewModel vm)
                {
                    List<DateTime> FechasConEventos = vm.FechasConEventos;

                    foreach (var item in UIHelper.FindVisualChildren<CalendarDayButton>(EventosCalendar))
                    {
                        if (item.DataContext is DateTime fecha && FechasConEventos.Contains(fecha.Date))
                        {
                            item.Background = Brushes.LightBlue;
                            item.FontWeight = FontWeights.Bold;
                        }
                        else
                        {
                            item.ClearValue(BackgroundProperty);
                            item.ClearValue(FontWeightProperty);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
