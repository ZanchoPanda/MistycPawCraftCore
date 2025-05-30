using MistycPawCraftCore.ViewModel;
using System;
using System.Windows;

namespace MistycPawCraftCore.View
{
    /// <summary>
    /// Lógica de interacción para RecoverDeckWindow.xaml
    /// </summary>
    public partial class RecoverDeckWindow : Window
    {
        public RecoverDeckWindow()
        {
            InitializeComponent();
        }

        private void RecoverClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RecoverDeckViewModel view = (RecoverDeckViewModel)this.DataContext;
                view.Recover = true;
                Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CancelRecoverClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RecoverDeckViewModel view = (RecoverDeckViewModel)this.DataContext;
                view.Recover = false;
                Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RecoverDeckViewModel view = (RecoverDeckViewModel)this.DataContext;
                view.AddRegisterToRecoveryList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                RecoverDeckViewModel view = (RecoverDeckViewModel)this.DataContext;
                view.RemoveRegisterToRecoveryList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
