using MistycPawCraftCore.Utils;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MistycPawCraftCore.View
{
    /// <summary>
    /// Lógica de interacción para DeckName.xaml
    /// </summary>
    public partial class DeckNameView : Window, INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Properties

        private string _DeckName;
        public string DeckName
        {
            get
            {
                return _DeckName;
            }
            set
            {
                if (value != _DeckName)
                {
                    _DeckName = value;
                    OnPropertyChanged("DeckName");
                }
            }
        }

        private string _FullDecksPath;
        public string FullDecksPath
        {
            get
            {
                return _FullDecksPath;
            }
            set
            {
                if (value != _FullDecksPath)
                {
                    _FullDecksPath = value;
                    OnPropertyChanged("FullDecksPath");
                }
            }
        }

        private bool _NombreAceptado;
        public bool NombreAceptado
        {
            get
            {
                return _NombreAceptado;
            }
            set
            {
                if (value != _NombreAceptado)
                {
                    _NombreAceptado = value;
                    OnPropertyChanged("NombreAceptado");
                }
            }
        }

        #endregion

        public DeckNameView()
        {
            DataContext = this;
            InitializeComponent();
        }

        public DeckNameView(string DecksPath)
        {
            DataContext = this;
            InitializeComponent();
            FullDecksPath = DecksPath;
        }


        private ICommand _AceptarCommand;
        public ICommand AceptarCommand
        {
            get
            {
                _AceptarCommand = new CommandHandler(() => AceptarAction(), true);
                return _AceptarCommand;
            }
        }


        private ICommand _CancelarCommand;
        public ICommand CancelarCommand
        {
            get
            {
                _CancelarCommand = new CommandHandler(() => CancelarAction(), true);
                return _CancelarCommand;
            }
        }

        private void CancelarAction()
        {
            try
            {
                NombreAceptado = false;
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AceptarAction()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DeckName))
                {
                    MessageBox.Show("You should write any name for save deck.");
                    return;
                }
                else
                {
                    if (File.Exists($"{FullDecksPath}\\{DeckName}.json"))
                    {
                        MessageBox.Show($"Exists one deck with the same Name.{Environment.NewLine}Choose another one.");
                        return;
                    }
                    else
                    {
                        NombreAceptado = true;
                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
