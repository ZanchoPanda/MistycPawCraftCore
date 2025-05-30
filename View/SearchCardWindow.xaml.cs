using MistycPawCraftCore.DTO;
using MistycPawCraftCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MistycPawCraftCore.View
{
    /// <summary>
    /// Lógica de interacción para SearchCardWindow.xaml
    /// </summary>
    public partial class SearchCardWindow : Window, INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Properties

        #region API

        private ScryfallApiClient _API;
        public ScryfallApiClient API
        {
            get
            {
                return _API;
            }
            set
            {
                if (value != _API)
                {
                    _API = value;
                    OnPropertyChanged("API");
                }
            }
        }

        #endregion

        private string _CardToSearch;
        public string CardToSearch
        {
            get
            {
                return _CardToSearch;
            }
            set
            {
                if (value != _CardToSearch)
                {
                    _CardToSearch = value;
                    OnPropertyChanged("CardToSearch");
                }
            }
        }

        private bool _MoreThanOne;
        public bool MoreThanOne
        {
            get
            {
                return _MoreThanOne;
            }
            set
            {
                if (value != _MoreThanOne)
                {
                    _MoreThanOne = value;
                    OnPropertyChanged("MoreThanOne");
                    AdjustWindowSize();
                }
            }
        }

        private ObservableCollection<CardDto> _ListaCartas;
        public ObservableCollection<CardDto> ListaCartas
        {
            get
            {
                return _ListaCartas;
            }
            set
            {
                if (value != _ListaCartas)
                {
                    _ListaCartas = value;
                    OnPropertyChanged("ListaCartas");
                }
            }
        }

        private CardDto _CartaSeleccionada;
        public CardDto CartaSeleccionada
        {
            get
            {
                return _CartaSeleccionada;
            }
            set
            {
                if (value != _CartaSeleccionada)
                {
                    _CartaSeleccionada = value;
                    OnPropertyChanged("CartaSeleccionada");
                }
            }
        }

        #endregion

        public SearchCardWindow()
        {
            DataContext = this;
            InitializeComponent();

            API = new ScryfallApiClient(true);
            ListaCartas = new ObservableCollection<CardDto>();

            if (SearchBox != null)
            {
                SearchBox.Focus();
            }
        }

        public SearchCardWindow(ScryfallApiClient AppScryfall)
        {
            DataContext = this;
            InitializeComponent();

            if (AppScryfall != null)
            {
                API = AppScryfall;
            }
            else
            {
                API = new ScryfallApiClient(true);
            }
            ListaCartas = new ObservableCollection<CardDto>();

            if (SearchBox != null)
            {
                SearchBox.Focus();
            }
        }

        #region Commands


        private ICommand _SearchCardCommand;
        public ICommand SearchCardCommand
        {
            get
            {
                _SearchCardCommand = new CommandHandler(() => SearchCardAction(), true);
                return _SearchCardCommand;
            }
        }

        private ICommand _CancelAddCardCommand;
        public ICommand CancelAddCardCommand
        {
            get
            {
                _CancelAddCardCommand = new CommandHandler(() => CancelAddCardAction(), true);
                return _CancelAddCardCommand;
            }
        }

        #endregion

        #region Function + Actions

        private async void SearchCardAction()
        {
            try
            {

                if (string.IsNullOrWhiteSpace(CardToSearch))
                {
                    return;
                }

                CartaSeleccionada = null;
                ListaCartas.Clear();
                MoreThanOne = false;

                Mouse.OverrideCursor = Cursors.Wait;

                List<CardDto> ListaCartasDevolucion = await API.GetCardByName(CardToSearch);
                if (ListaCartasDevolucion != null && ListaCartasDevolucion.Count > 0)
                {
                    if (ListaCartasDevolucion.Count == 1)
                    {
                        CartaSeleccionada = ListaCartasDevolucion.FirstOrDefault();
                        this.Close();
                    }
                    else
                    {
                        ListaCartasDevolucion.ForEach(k => ListaCartas.Add(k));
                        MoreThanOne = true;
                    }

                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Cards not found with that name.");
                    CardToSearch = string.Empty;
                }

            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = null;
                AdjustWindowSize();
            }
        }

        private void AdjustWindowSize()
        {
            try
            {
                if (MoreThanOne)
                {
                    Height = 500;
                }
                else
                {
                    Height = 100;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CancelAddCardAction()
        {
            try
            {
                CartaSeleccionada = null;
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (CartaSeleccionada == null)
                {
                    return;
                }
                else
                {
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
