using MistycPawCraftCore.DTO;
using MistycPawCraftCore.DTO.Filtros;
using MistycPawCraftCore.Utils;
using MistycPawCraftCore.Utils.Enums;
using MistycPawCraftCore.Utils.Helpers;
using MistycPawCraftCore.Utils.Language;
using MistycPawCraftCore.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MistycPawCraftCore.ViewModel
{
    public class PrincipalViewModel : INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

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

        public PrincipalViewModel()
        {
            try
            {
                API = new ScryfallApiClient(true);

                MtgCardTypes = EnumHelper.GetValues<MtgCardType>();
                OrderTypes = EnumHelper.GetValues<OrderTypesEnum>();

                #region OrderList
                //if (OrderList == null)
                //{
                //    OrderList = new List<OrderTypesEnum>();
                //}
                //foreach (var item in Enum.GetValues(typeof(OrderTypesEnum)))
                //{
                //    switch (item)
                //    {
                //        case OrderTypesEnum.Name:
                //            OrderList.Add(OrderTypesEnum.Name);
                //            break;
                //        case OrderTypesEnum.Release_date:
                //            OrderList.Add(OrderTypesEnum.Release_date);
                //            break;
                //        case OrderTypesEnum.Set_Number:
                //            OrderList.Add(OrderTypesEnum.Set_Number);
                //            break;
                //        case OrderTypesEnum.Rarity:
                //            OrderList.Add(OrderTypesEnum.Rarity);
                //            break;
                //        case OrderTypesEnum.Color:
                //            OrderList.Add(OrderTypesEnum.Color);
                //            break;
                //        case OrderTypesEnum.Price_EUR:
                //            OrderList.Add(OrderTypesEnum.Price_EUR);
                //            break;
                //        case OrderTypesEnum.Price_USD:
                //            OrderList.Add(OrderTypesEnum.Price_USD);
                //            break;
                //        case OrderTypesEnum.Mana_Cost:
                //            OrderList.Add(OrderTypesEnum.Mana_Cost);
                //            break;
                //        case OrderTypesEnum.Power:
                //            OrderList.Add(OrderTypesEnum.Power);
                //            break;
                //        case OrderTypesEnum.Thoughness:
                //            OrderList.Add(OrderTypesEnum.Thoughness);
                //            break;
                //        case OrderTypesEnum.Artist:
                //            OrderList.Add(OrderTypesEnum.Artist);
                //            break;
                //        case OrderTypesEnum.Set_Review:
                //            OrderList.Add(OrderTypesEnum.Set_Review);
                //            break;
                //        default:
                //            break;
                //    }
                //}
                #endregion

                Filtros = new FiltroDTO();
                IsExpandedProp = false;
                CardsReprintsVisibility = Visibility.Collapsed;
                HayCartaSeleccionada = Visibility.Collapsed;

                CargarManaSymbols();

                LocalizationManager.LanguageChanged += (s, e) =>
                {
                    MtgCardTypes = EnumHelper.GetValues<MtgCardType>();
                    OrderTypes = EnumHelper.GetValues<OrderTypesEnum>();
                    OnPropertyChanged(nameof(MtgCardTypes));
                    OnPropertyChanged(nameof(OrderTypes));
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CargarManaSymbols()
        {
            try
            {
                ManaBasePath = $"{Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName}\\Images\\Symbol\\";
                ManaW = $"{ManaBasePath}W.png";
                ManaU = $"{ManaBasePath}U.png";
                ManaB = $"{ManaBasePath}B.png";
                ManaR = $"{ManaBasePath}R.png";
                ManaG = $"{ManaBasePath}G.png";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Propiedades

        #region Enums

        //public List<EnumItem<MtgCardType>> MtgCardTypes { get; set; }
        private List<EnumItem<MtgCardType>> _MtgCardTypes;
        public List<EnumItem<MtgCardType>> MtgCardTypes
        {
            get
            {
                return _MtgCardTypes;
            }
            set
            {
                if (value != _MtgCardTypes)
                {
                    _MtgCardTypes = value;
                    OnPropertyChanged("MtgCardTypes");
                }
            }
        }

        private List<EnumItem<OrderTypesEnum>> _OrderTypes;
        public List<EnumItem<OrderTypesEnum>> OrderTypes
        {
            get
            {
                return _OrderTypes;
            }
            set
            {
                if (value != _OrderTypes)
                {
                    _OrderTypes = value;
                    OnPropertyChanged("OrderTypes");
                }
            }
        }


        #endregion

        #region Mana
        private string _ManaBasePath;
        public string ManaBasePath
        {
            get
            {
                return _ManaBasePath;
            }
            set
            {
                if (value != _ManaBasePath)
                {
                    _ManaBasePath = value;
                    OnPropertyChanged("ManaBasePath");
                }
            }
        }
        private string _ManaW;
        public string ManaW
        {
            get
            {
                return _ManaW;
            }
            set
            {
                if (value != _ManaW)
                {
                    _ManaW = value;
                    OnPropertyChanged("ManaW");
                }
            }
        }
        private string _ManaU;
        public string ManaU
        {
            get
            {
                return _ManaU;
            }
            set
            {
                if (value != _ManaU)
                {
                    _ManaU = value;
                    OnPropertyChanged("ManaU");
                }
            }
        }
        private string _ManaB;
        public string ManaB
        {
            get
            {
                return _ManaB;
            }
            set
            {
                if (value != _ManaB)
                {
                    _ManaB = value;
                    OnPropertyChanged("ManaB");
                }
            }
        }
        private string _ManaR;
        public string ManaR
        {
            get
            {
                return _ManaR;
            }
            set
            {
                if (value != _ManaR)
                {
                    _ManaR = value;
                    OnPropertyChanged("ManaR");
                }
            }
        }
        private string _ManaG;
        public string ManaG
        {
            get
            {
                return _ManaG;
            }
            set
            {
                if (value != _ManaG)
                {
                    _ManaG = value;
                    OnPropertyChanged("ManaG");
                }
            }
        }
        #endregion


        private string _TextCard;
        public string TextCard
        {
            get
            {
                return _TextCard;
            }
            set
            {
                if (value != _TextCard)
                {
                    _TextCard = value;
                    OnPropertyChanged("TextCard");
                }
            }
        }

        private bool _IsExpandedProp;
        public bool IsExpandedProp
        {
            get
            {
                return _IsExpandedProp;
            }
            set
            {
                if (value != _IsExpandedProp)
                {
                    _IsExpandedProp = value;
                    OnPropertyChanged("IsExpandedProp");
                }
            }
        }

        private List<OrderTypesEnum> _OrderList;
        public List<OrderTypesEnum> OrderList
        {
            get
            {
                return _OrderList;
            }
            set
            {
                if (value != _OrderList)
                {
                    _OrderList = value;
                    OnPropertyChanged("OrderList");
                }
            }
        }

        private List<MtgCardType> _CardTypesList;
        public List<MtgCardType> CardTypesList
        {
            get
            {
                return _CardTypesList;
            }
            set
            {
                if (value != _CardTypesList)
                {
                    _CardTypesList = value;
                    OnPropertyChanged("CardTypesList");
                }
            }
        }


        private FiltroDTO _Filtros;
        public FiltroDTO Filtros
        {
            get
            {
                return _Filtros;
            }
            set
            {
                if (value != _Filtros)
                {
                    _Filtros = value;
                    OnPropertyChanged("Filtros");
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
                    caraActualIndex = 1;
                    _CartaSeleccionada = value;
                    OnPropertyChanged("CartaSeleccionada");
                    if (CartaSeleccionada != null)
                    {
                        HayCartaSeleccionada = Visibility.Visible;

                        #region Reprints

                        if (CartaSeleccionada.HasReprints)
                        {
                            CardsReprintsVisibility = Visibility.Visible;
                        }
                        else
                        {
                            CardsReprintsVisibility = Visibility.Collapsed;
                        }
                        #endregion
                        #region Caras
                        if (CartaSeleccionada.CardFaces.Count > 0)
                        {
                            CardsFacesVisibility = Visibility.Visible;
                        }
                        else
                        {
                            CardsFacesVisibility = Visibility.Collapsed;
                        }
                        #endregion

                        AlternarCaraAction();
                    }
                    else
                    {
                        HayCartaSeleccionada = Visibility.Collapsed;
                    }
                    OnPropertyChanged("ImagenActual");
                }
            }
        }

        private Visibility _CardsReprintsVisibility;
        public Visibility CardsReprintsVisibility
        {
            get
            {
                return _CardsReprintsVisibility;
            }
            set
            {
                if (value != _CardsReprintsVisibility)
                {
                    _CardsReprintsVisibility = value;
                    OnPropertyChanged("CardsReprintsVisibility");
                }
            }
        }

        private Visibility _CardsFacesVisibility;
        public Visibility CardsFacesVisibility
        {
            get
            {
                return _CardsFacesVisibility;
            }
            set
            {
                if (value != _CardsFacesVisibility)
                {
                    _CardsFacesVisibility = value;
                    OnPropertyChanged("CardsFacesVisibility");
                }
            }
        }

        private Visibility _HayCartaSeleccionada;
        public Visibility HayCartaSeleccionada
        {
            get
            {
                return _HayCartaSeleccionada;
            }
            set
            {
                if (value != _HayCartaSeleccionada)
                {
                    _HayCartaSeleccionada = value;
                    OnPropertyChanged("HayCartaSeleccionada");
                }
            }
        }

        #endregion

        #region Commands

        private ICommand _OpenCardMarketCommand;
        public ICommand OpenCardMarketCommand
        {
            get
            {
                _OpenCardMarketCommand = new CommandHandler(() => OpenCardMarketAction(), true);
                return _OpenCardMarketCommand;
            }
        }

        private void OpenCardMarketAction()
        {
            try
            {
                if (CartaSeleccionada == null || CartaSeleccionada.PurchasesUri == null || string.IsNullOrWhiteSpace(CartaSeleccionada.PurchasesUri.cardmarket.ToString()))
                {
                    return;
                }

                //Process.Start(CartaSeleccionada.PurchasesUri.cardmarket.ToString());
                Process.Start(new ProcessStartInfo
                {
                    FileName = CartaSeleccionada.PurchasesUri.cardmarket.ToString(),
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ICommand _OpenTradingCardPlayerCommand;
        public ICommand OpenTradingCardPlayerCommand
        {
            get
            {
                _OpenTradingCardPlayerCommand = new CommandHandler(() => OpenTradingCardPlayerAction(), true);
                return _OpenTradingCardPlayerCommand;
            }
        }

        private void OpenTradingCardPlayerAction()
        {
            try
            {
                if (CartaSeleccionada == null || CartaSeleccionada.PurchasesUri == null || string.IsNullOrWhiteSpace(CartaSeleccionada.PurchasesUri.tcgplayer.ToString()))
                {
                    return;
                }

                //Process.Start(CartaSeleccionada.PurchasesUri.tcgplayer.ToString());
                Process.Start(new ProcessStartInfo
                {
                    FileName = CartaSeleccionada.PurchasesUri.tcgplayer.ToString(),
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ICommand _OpenCardHoaderCommand;
        public ICommand OpenCardHoaderCommand
        {
            get
            {
                _OpenCardHoaderCommand = new CommandHandler(() => OpenCardHoaderAction(), true);
                return _OpenCardHoaderCommand;
            }
        }

        private void OpenCardHoaderAction()
        {
            try
            {
                if (CartaSeleccionada == null || CartaSeleccionada.PurchasesUri == null || string.IsNullOrWhiteSpace(CartaSeleccionada.PurchasesUri.cardhoader.ToString()))
                {
                    return;
                }

                //Process.Start(CartaSeleccionada.PurchasesUri.cardhoader.ToString());
                Process.Start(new ProcessStartInfo
                {
                    FileName = CartaSeleccionada.PurchasesUri.cardhoader.ToString(),
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                _SearchCommand = new CommandHandler(() => SearchActionAsync(), true);
                return _SearchCommand;
            }
        }

        private async Task SearchActionAsync()
        {
            try
            {
                if (API == null)
                {
                    API = new ScryfallApiClient(true);
                }

                if (Filtros == null)
                {
                    Filtros = new FiltroDTO();
                    return;
                }

                //if (string.IsNullOrWhiteSpace(Filtros.Name))
                //{
                //    MessageBox.Show("You should write any in Text field to search.");
                //    return;
                //}

                CartaSeleccionada = null;
                Mouse.OverrideCursor = Cursors.Wait;

                List<CardDto> ListaCartasNueva = await API.GetCardsWithFilters(Filtros);

                //List<CardDto> ListaCartasNueva = await API.GetCardsByName(TextCard);
                if (ListaCartasNueva != null && ListaCartasNueva.Count > 0)
                {
                    CartaSeleccionada = null;
                    ListaCartas = new ObservableCollection<CardDto>(ListaCartasNueva);
                    IsExpandedProp = false;

                    if (ListaCartas.Count == 1)
                    {
                        CartaSeleccionada = ListaCartas.FirstOrDefault();
                    }

                }
                else
                {
                    MessageBox.Show("No cards finded with filters given. Try with anothers.");
                    CartaSeleccionada = null;
                    ListaCartas?.Clear();
                    IsExpandedProp = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private ICommand _ShowReprintsCommand;
        public ICommand ShowReprintsCommand
        {
            get
            {
                _ShowReprintsCommand = new CommandHandler(() => ShowReprintsActionAsync(), true);
                return _ShowReprintsCommand;
            }
        }

        private async Task ShowReprintsActionAsync()
        {
            try
            {
                if (CartaSeleccionada != null && CartaSeleccionada.HasReprints)
                {
                    #region Anterior, funcional pero lento
                    //Mouse.OverrideCursor = Cursors.Wait;
                    //List<ReprintsDTO> Lista = await _API.FillReprints(CartaSeleccionada.RePrintsUri);
                    //Lista = Lista.OrderByDescending(k => k.ReleaseAt).ToList();
                    //Mouse.OverrideCursor = null;
                    //ReprintsViewModel ReprintsVM = new ReprintsViewModel(Lista);
                    //ReprintsView WindowReprints = new ReprintsView() { DataContext = ReprintsVM };
                    ////WindowReprints.DataContext = ReprintsVM;
                    //WindowReprints.ShowDialog();
                    #endregion

                    #region Nuevo, testeando

                    Mouse.OverrideCursor = Cursors.Wait;

                    // Creas la instancia del ViewModel pasando la URI y la API
                    var reprintsVM = new ReprintsViewModel(CartaSeleccionada.RePrintsUri, _API);

                    // Creas la ventana y le asignas el DataContext
                    var windowReprints = new ReprintsView
                    {
                        DataContext = reprintsVM
                    };

                    // Quitas el cursor de espera
                    Mouse.OverrideCursor = null;

                    // Muestras la ventana modal
                    windowReprints.ShowDialog();

                    #endregion

                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Selected card hasn't got any reprint.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        private ICommand _AlternarCaraCommand;
        public ICommand AlternarCaraCommand
        {
            get
            {
                _AlternarCaraCommand = new CommandHandler(() => AlternarCaraAction(), true);
                return _AlternarCaraCommand;
            }
        }

        private int caraActualIndex = 0;
        private string _ImagenActual;
        public string ImagenActual
        {
            get
            {
                return _ImagenActual;
            }
            set
            {
                if (value != _ImagenActual)
                {
                    _ImagenActual = value;
                    OnPropertyChanged("ImagenActual");
                }
            }
        }

        private void AlternarCaraAction()
        {
            if (CartaSeleccionada?.CardFaces?.Count > 1)
            {
                caraActualIndex = (caraActualIndex == 0) ? 1 : 0;
                ImagenActual = CartaSeleccionada.CardFaces[caraActualIndex].image_uris.normal;
            }
            else
            {
                ImagenActual = CartaSeleccionada?.ImageUris.Normal.ToString();
            }

            OnPropertyChanged(nameof(ImagenActual));
        }


        #endregion

        #region Events

        #endregion

    }
}
