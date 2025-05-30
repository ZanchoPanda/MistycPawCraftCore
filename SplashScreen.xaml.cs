using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;

namespace MistycPawCraftCore
{
    /// <summary>
    /// Lógica de interacción para SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_Completed;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                ProgressBar.Value = e.ProgressPercentage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (ProgressBar.Value == 100)
                {
                    Close();
                    MainWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        MainWindow MainWindow { get; set; }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                for (int i = 0; i <= 100; i++)
                {
                    Thread.Sleep(40);

                    (sender as BackgroundWorker).ReportProgress(i);
                    if (MainWindow == null)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            MainWindow = new MainWindow();
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

    }
}
