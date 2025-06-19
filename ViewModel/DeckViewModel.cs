using Microsoft.Win32;
using MistycPawCraftCore.DTO;
using MistycPawCraftCore.Properties;
using MistycPawCraftCore.Utils;
using MistycPawCraftCore.Utils.Enums;
using MistycPawCraftCore.Utils.Language;
using MistycPawCraftCore.Utils.Message;
using MistycPawCraftCore.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MistycPawCraftCore.ViewModel
{
    public class DeckViewModel : INotifyPropertyChanged
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

        #region Constructor

        public DeckViewModel()
        {
            try
            {
                InitiateAPI(false);

                BasePath = Settings.Default.ImageBasePath;
                //BasePath = Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;
                FullDecksPath = $"{BasePath}\\Decks";
                Directory.CreateDirectory(FullDecksPath);

                ListaDecks = new ObservableCollection<DeckDTO>();
                IsSelectedDeck = Visibility.Collapsed;
                CheckAllSavedDecks();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckAllSavedDecks()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullDecksPath))
                {
                    return;
                }

                if (ListaDecks == null)
                {
                    return;
                }

                ListaDecks.Clear();

                foreach (string FileString in Directory.GetFiles(FullDecksPath, "*.json"))
                {
                    DeckDTO DeckLeido = JsonConvert.DeserializeObject<DeckDTO>(File.ReadAllText(FileString));
                    if (DeckLeido.DeleteDate.HasValue)
                    {
                        continue;
                    }
                    else
                    {
                        ListaDecks.Add(DeckLeido);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Properties

        public string BasePath { get; set; }
        public string FullDecksPath { get; set; }

        private int _DeckTotalCount;
        public int DeckTotalCount
        {
            get
            {
                return _DeckTotalCount;
            }
            set
            {
                if (value != _DeckTotalCount)
                {
                    _DeckTotalCount = value;
                    OnPropertyChanged("DeckTotalCount");
                }
            }
        }

        private int _SideboardTotalCount;
        public int SideboardTotalCount
        {
            get
            {
                return _SideboardTotalCount;
            }
            set
            {
                if (value != _SideboardTotalCount)
                {
                    _SideboardTotalCount = value;
                    OnPropertyChanged("SideboardTotalCount");
                }
            }
        }

        private ObservableCollection<DeckDTO> _ListaDecks;
        public ObservableCollection<DeckDTO> ListaDecks
        {
            get
            {
                return _ListaDecks;
            }
            set
            {
                if (value != _ListaDecks)
                {
                    _ListaDecks = value;
                    OnPropertyChanged("ListaDecks");
                }
            }
        }

        private DeckDTO _DeckSeleccionado;
        public DeckDTO DeckSeleccionado
        {
            get
            {
                return _DeckSeleccionado;
            }
            set
            {
                if (value != _DeckSeleccionado)
                {
                    _DeckSeleccionado = value;
                    OnPropertyChanged("DeckSeleccionado");
                    CheckDeckVisibility();
                    if (DeckSeleccionado != null)
                    {
                        DeckSeleccionado.PropertyChanged += DeckChange;
                        DeckSeleccionado.Deck.ToList().ForEach(k => k.PropertyChanged += DeckChanged);
                        DeckSeleccionado.SideBoard.ToList().ForEach(k => k.PropertyChanged += DeckChanged);
                        DeckSeleccionado.CalculatePriceDeck();
                    }
                }
            }
        }

        private void DeckChange(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == nameof(DeckSeleccionado.Deck) || e.PropertyName == nameof(DeckSeleccionado.SideBoard))
                {
                    DeckSeleccionado.CheckDeckStats();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool _ExpansionActivated;
        public bool ExpansionActivated
        {
            get
            {
                return _ExpansionActivated;
            }
            set
            {
                if (value != _ExpansionActivated)
                {
                    _ExpansionActivated = value;
                    OnPropertyChanged("ExpansionActivated");
                }
            }
        }

        private void DeckChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                DeckSeleccionado.CheckDeckStats();
                SaveChangesDeck(DeckSeleccionado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CambioDeck(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                SaveChangesDeck(DeckSeleccionado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckDeckVisibility()
        {
            try
            {
                if (DeckSeleccionado == null)
                {
                    IsSelectedDeck = Visibility.Collapsed;
                    ExpansionActivated = false;
                }
                else
                {
                    IsSelectedDeck = Visibility.Visible;
                    ExpansionActivated = (DeckSeleccionado.SideboardCount > 0) ? true : false;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Visibility _IsSelectedDeck;

        public Visibility IsSelectedDeck
        {
            get
            {
                return _IsSelectedDeck;
            }
            set
            {
                if (value != _IsSelectedDeck)
                {
                    _IsSelectedDeck = value;
                    OnPropertyChanged("IsSelectedDeck");
                }
            }
        }


        private CardDto _SelectedCard;
        public CardDto SelectedCard
        {
            get
            {
                return _SelectedCard;
            }
            set
            {
                if (value != _SelectedCard)
                {
                    _SelectedCard = value;
                    OnPropertyChanged("SelectedCard");
                    if (SelectedCard != null)
                    {
                        SelectedCard.PropertyChanged += DeckChanged;
                    }
                }
            }
        }

        private CardDto _SelectedSideboardCard;
        public CardDto SelectedSideboardCard
        {
            get
            {
                return _SelectedSideboardCard;
            }
            set
            {
                if (value != _SelectedSideboardCard)
                {
                    _SelectedSideboardCard = value;
                    OnPropertyChanged("SelectedSideboardCard");
                    if (SelectedSideboardCard != null)
                    {
                        SelectedSideboardCard.PropertyChanged += DeckChanged;
                    }
                }
            }
        }

        #endregion

        #region Commands


        private ICommand _NewDeckCommand;
        public ICommand NewDeckCommand
        {
            get
            {
                _NewDeckCommand = new CommandHandler(() => NewDeckAction(), true);
                return _NewDeckCommand;
            }
        }

        private ICommand _DeleteDeckCommand;
        public ICommand DeleteDeckCommand
        {
            get
            {
                _DeleteDeckCommand = new CommandHandler(() => DeleteDeckAction(), true);
                return _DeleteDeckCommand;
            }
        }

        private ICommand _ExportDeckCommand;
        public ICommand ExportDeckCommand
        {
            get
            {
                _ExportDeckCommand = new CommandHandler(() => ExportDeckAction(), true);
                return _ExportDeckCommand;
            }
        }

        private ICommand _ImportDeckCommand;
        public ICommand ImportDeckCommand
        {
            get
            {
                _ImportDeckCommand = new CommandHandler(() => ImportDeckAction(), true);
                return _ImportDeckCommand;
            }
        }

        #region MainDeck

        private ICommand _AddCardMainDeckCommmand;
        public ICommand AddCardMainDeckCommmand
        {
            get
            {
                _AddCardMainDeckCommmand = new CommandHandler(() => AddCardMainDeckActionAsync(), true);
                return _AddCardMainDeckCommmand;
            }
        }


        private ICommand _DeleteMainCardCommand;
        public ICommand DeleteMainCardCommand
        {
            get
            {
                _DeleteMainCardCommand = new CommandHandler(() => DeleteMainCardAction(), true);
                return _DeleteMainCardCommand;
            }
        }

        #endregion

        #region Sideboard

        private ICommand _AddCardSideboardDeckCommand;
        public ICommand AddCardSideboardDeckCommand
        {
            get
            {
                _AddCardSideboardDeckCommand = new CommandHandler(() => AddCardSideboardDeckAction(), true);
                return _AddCardSideboardDeckCommand;
            }
        }

        private ICommand _DeleteCardSideboardDeckCommand;
        public ICommand DeleteCardSideboardDeckCommand
        {
            get
            {
                _DeleteCardSideboardDeckCommand = new CommandHandler(() => DeleteCardSideboardDeckAction(), true);
                return _DeleteCardSideboardDeckCommand;
            }
        }

        private ICommand _RecoverDeletedDecksCommand;
        public ICommand RecoverDeletedDecksCommand
        {
            get
            {
                _RecoverDeletedDecksCommand = new CommandHandler(() => RecoverDeletedDecksAction(), true);
                return _RecoverDeletedDecksCommand;
            }
        }

        #endregion

        #endregion

        #region Actions

        private void NewDeckAction()
        {
            try
            {
                DeckNameView Ventana = new DeckNameView(FullDecksPath);
                Ventana.ShowDialog();

                if (!Ventana.NombreAceptado)
                {
                    return;
                }

                DeckDTO NuevoDeck = new DeckDTO();
                NuevoDeck.DeckName = Ventana.DeckName;
                NuevoDeck.CreateDate = DateTime.Now;

                try
                {
                    SaveChangesDeck(NuevoDeck);
                    ListaDecks.Add(NuevoDeck);
                    DeckSeleccionado = ListaDecks.FirstOrDefault(k => k == NuevoDeck);
                }
                catch (Exception)
                {

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeleteDeckAction()
        {
            try
            {

                if (DeckSeleccionado == null || (DeckSeleccionado != null && DeckSeleccionado.DeleteDate.HasValue))
                {
                    return;
                }

                string Mensaje1 = $"{LocalizationManager.GetString("DeleteDeckConfirmationLine1")}";

                CustomDialog cd = new CustomDialog(EnumTitleMessage.Ask, Mensaje1, EnumButtonsMessage.YesNo);
                cd.ShowDialog();

                //if (MessageBox.Show("Do you wanna delete selected deck?", "Delete Deck", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                if (cd.MessageResult == EnumMessageResult.Yes)
                {
                    DeckSeleccionado.DeleteDate = DateTime.Now;
                    SaveChangesDeck(DeckSeleccionado);

                    Mensaje1 = $"{LocalizationManager.GetString("DeleteDeckConfirmationLine2")}";
                    CustomDialog cd2 = new CustomDialog(EnumTitleMessage.Ask, Mensaje1, EnumButtonsMessage.YesNo);
                    cd2.ShowDialog();

                    if (cd2.MessageResult == EnumMessageResult.Yes)
                    {
                        DeleteFileDeck(DeckSeleccionado);
                    }
                    else
                    {
                        HideDeck(DeckSeleccionado);
                    }

                    if (ListaDecks.Remove(DeckSeleccionado))
                    {
                        DeckSeleccionado = null;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RecoverDeletedDecksAction()
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(FullDecksPath);
                List<FileInfo> HiddenFiles = dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(file => (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden).ToList();

                if (HiddenFiles.Count == 0)
                {
                    MessageBox.Show("Any deck is deleted or can't be recover.");
                    return;
                }

                List<DeckDTO> DeletedListDeck = new List<DeckDTO>();
                foreach (FileInfo fileFound in HiddenFiles)
                {
                    DeckDTO DeckLeido = JsonConvert.DeserializeObject<DeckDTO>(File.ReadAllText(fileFound.FullName));
                    DeletedListDeck.Add(DeckLeido);
                }

                RecoverDeckWindow RecovWindow = new RecoverDeckWindow();
                RecoverDeckViewModel ViewModel = (RecoverDeckViewModel)RecovWindow.DataContext;
                if (ViewModel != null)
                {
                    DeletedListDeck.ForEach(k => ViewModel.RecoverDecks.Add(k));
                }
                RecovWindow.ShowDialog();

                if (ViewModel.Recover)
                {
                    foreach (DeckDTO RecoveredDeck in ViewModel.SelectedDecks)
                    {
                        RecoverDeck(RecoveredDeck);
                    }
                    ListaDecks.Clear();
                    CheckAllSavedDecks();

                    if (ViewModel.SelectedDecks.Count == 1)
                    {
                        DeckSeleccionado = ListaDecks.FirstOrDefault(k => k.DeckName == ViewModel.SelectedDecks.FirstOrDefault().DeckName);
                    }

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExportDeckAction()
        {
            try
            {
                if (DeckSeleccionado == null || (DeckSeleccionado != null && DeckSeleccionado.DeleteDate.HasValue))
                {
                    return;
                }

                SaveFileDialog SaveFile = new SaveFileDialog();
                SaveFile.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
                SaveFile.FileName = DeckSeleccionado.DeckName;
                SaveFile.AddExtension = false;
                SaveFile.Filter = "Text Files (*.txt)|*.txt";
                bool ResultadoOK = SaveFile.ShowDialog() ?? false;

                if (ResultadoOK)
                {
                    try
                    {
                        string v = (string.IsNullOrWhiteSpace(System.IO.Path.GetExtension(SaveFile.FileName))) ? $"{SaveFile.FileName}.txt" : SaveFile.FileName;
                        using (StreamWriter writer = new StreamWriter($"{v}", false, Encoding.UTF8))
                        {
                            // Write a header for the main deck
                            writer.WriteLine("Main");

                            // Write each main deck card to the file
                            foreach (CardDto MainCard in DeckSeleccionado.Deck.OrderByDescending(k => k.Set.ReleasedDate).ThenBy(k => k.TypeAndClass.CardType.FullCardType))
                            {
                                if (MainCard.Units == 0)
                                {
                                    continue;
                                }
                                writer.WriteLine($"{MainCard.Units} {MainCard.Name} ({MainCard.Set.CodeSet})");
                            }

                            if (DeckSeleccionado.SideBoard != null)
                            {
                                // Write a header for the sideboard
                                writer.WriteLine("Sideboard");

                                // Write each sideboard card to the file
                                foreach (CardDto SideboardCard in DeckSeleccionado.SideBoard.OrderByDescending(k => k.Set.ReleasedDate).ThenBy(k => k.TypeAndClass.CardType.FullCardType))
                                {
                                    if (SideboardCard.Units == 0)
                                    {
                                        continue;
                                    }
                                    writer.WriteLine($"{SideboardCard.Units} {SideboardCard.Name} ({SideboardCard.Set.CodeSet})");
                                }
                            }
                        }

                        Clipboard.SetText(File.ReadAllText(v));

                        if (MessageBox.Show($"Deck copied to clipboard.{Environment.NewLine}Do you wanna open created file?", string.Empty, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = SaveFile.FileName,
                                UseShellExecute = true
                            });
                        }
                    }
                    catch (Exception exc)
                    {
                        var aux = exc;
                        Console.WriteLine(exc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async void ImportDeckAction()
        {
            try
            {

                ImportWindow Imp = new ImportWindow();
                Imp.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                Imp.ShowDialog();

                if (!string.IsNullOrWhiteSpace(((ImportViewModel)Imp.DataContext).DeckList))
                {
                    CheckAllSavedDecks();
                }

                #region Anterior

                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine($"Cards not founded or wrong tiped:");

                //OpenFileDialog OFileDialog = new OpenFileDialog();
                //OFileDialog.InitialDirectory = "c:\\";
                //OFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                //OFileDialog.FilterIndex = 2;
                //OFileDialog.RestoreDirectory = true;

                //string filepath;

                //if (OFileDialog.ShowDialog().Value)
                //{
                //    Mouse.OverrideCursor = Cursors.Wait;

                //    filepath = OFileDialog.FileName;

                //    List<CardDto> Cards = new List<CardDto>();
                //    try
                //    {
                //        if (API == null)
                //        {
                //            InitiateAPI(false);
                //        }

                //        DeckDTO ImportedDeck = new DeckDTO();
                //        ImportedDeck.DeckName = System.IO.Path.GetFileNameWithoutExtension(filepath);

                //        string[] lines = File.ReadAllLines(filepath);
                //        bool isMainDeck = true;
                //        foreach (string Line in lines)
                //        {
                //            if (Line == "Main" | Line == "Deck")
                //            {
                //                isMainDeck = true;
                //                ImportedDeck.Deck = ImportedDeck.Deck == null ? new ObservableCollection<CardDto>() : ImportedDeck.Deck;
                //                continue;
                //            }

                //            if (Line == "Sideboard")
                //            {
                //                isMainDeck = false;
                //                ImportedDeck.SideBoard = ImportedDeck.SideBoard == null ? new ObservableCollection<CardDto>() : ImportedDeck.SideBoard;
                //                continue;
                //            }

                //            string[] Parts = Line.Split(' ');

                //            if (Parts.Length >= 2)
                //            {
                //                int quantity = 0;
                //                if (Parts[0].All(char.IsDigit))
                //                {
                //                    int.TryParse(Parts[0], out quantity);
                //                }
                //                else
                //                {
                //                    string numberOnly = Regex.Replace(Parts[0], "[^0-9.]", "");
                //                    int.TryParse(numberOnly, out quantity);
                //                }


                //                if (quantity != 0)
                //                {
                //                    string name = string.Join(" ", Parts.Skip(1));

                //                    name = API.CleanNameCard(name);

                //                    List<CardDto> SearchedCard = await API.GetCardByName(name);

                //                    if (SearchedCard != null)
                //                    {

                //                        CardDto card = SearchedCard.FirstOrDefault();
                //                        card.Units = quantity;

                //                        if (isMainDeck)
                //                        {
                //                            ImportedDeck.Deck.Add(card);
                //                        }
                //                        else
                //                        {
                //                            ImportedDeck.SideBoard.Add(card);
                //                        }

                //                    }
                //                    else
                //                    {
                //                        sb.AppendLine($"{name}");
                //                    }
                //                }
                //            }

                //        }

                //        ImportedDeck.CheckDeckStats();
                //        SaveChangesDeck(ImportedDeck);

                //        Mouse.OverrideCursor = null;
                //        MessageBox.Show("Deck imported correctly", "", MessageBoxButton.OK);

                //        ImportedDeck.PropertyChanged += CambioDeck;
                //        ListaDecks.Add(ImportedDeck);
                //        DeckSeleccionado = ListaDecks.FirstOrDefault(k => k == ImportedDeck);
                //    }
                //    catch (Exception exp)
                //    {
                //        throw exp;
                //    }

                //    if (Regex.Matches(sb.ToString(), Environment.NewLine).Count > 1)
                //    {
                //        MessageBox.Show(sb.ToString());
                //    }

                //}
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ICommand _RefreshDecksCommand;
        public ICommand RefreshDecksCommand
        {
            get
            {
                _RefreshDecksCommand = new CommandHandler(() => ReloadIconsDecksAction(), true);
                return _RefreshDecksCommand;
            }
        }

        private void ReloadIconsDecksAction()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullDecksPath))
                {
                    return;
                }

                ListaDecks.Clear();
                //SelectedDeck = null;

                foreach (string FileString in Directory.GetFiles(FullDecksPath, "*.json"))
                {
                    DeckDTO DeckLeido = JsonConvert.DeserializeObject<DeckDTO>(File.ReadAllText(FileString));
                    if (DeckLeido.DeleteDate.HasValue)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (CardDto Carta in DeckLeido.Deck)
                        {
                            string SetName = Path.GetFileName(Carta.Set.ImageLocalPath);
                            Carta.Set.ImageLocalPath = (string.IsNullOrWhiteSpace(SetName)) ? Path.Combine(Settings.Default.ImageSetPath, Carta.Set.CodeSet, $"{Carta.Set.CodeSet}.png") : Path.Combine(Settings.Default.ImageSetPath, Carta.Set.CodeSet, SetName);

                            foreach (var ManaCostView in Carta.ManaCostView)
                            {
                                string CostName = Path.GetFileName(ManaCostView.SymbolPath);
                                ManaCostView.SymbolPath = Path.Combine(Settings.Default.ImageSymbolPath, CostName);
                            }
                        }

                        foreach (CardDto Carta in DeckLeido.SideBoard)
                        {
                            string SetName = Path.GetFileName(Carta.Set.ImageLocalPath);
                            Carta.Set.ImageLocalPath = (string.IsNullOrWhiteSpace(SetName)) ? Path.Combine(Settings.Default.ImageSetPath, Carta.Set.CodeSet, $"{Carta.Set.CodeSet}.png") : Path.Combine(Settings.Default.ImageSetPath, Carta.Set.CodeSet, SetName);

                            foreach (var ManaCostView in Carta.ManaCostView)
                            {
                                string CostName = Path.GetFileName(ManaCostView.SymbolPath);
                                ManaCostView.SymbolPath = Path.Combine(Settings.Default.ImageSymbolPath, CostName);
                            }
                        }

                        SaveChangesDeck(DeckLeido);
                        ListaDecks.Add(DeckLeido);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region MainDeck

        private async Task AddCardMainDeckActionAsync()
        {
            try
            {
                if (API == null)
                {
                    InitiateAPI(false);
                }

                SearchCardWindow SearchWindow = new SearchCardWindow(API);
                SearchWindow.ShowDialog();

                if (SearchWindow.CartaSeleccionada != null)
                {
                    if (DeckSeleccionado.Deck.Any(c => c.Name == SearchWindow.CartaSeleccionada.Name))
                    {
                        CardDto Carta = DeckSeleccionado.Deck.FirstOrDefault(k => k.Name == SearchWindow.CartaSeleccionada.Name);
                        if (Carta != null)
                        {
                            Carta.Units += 1;
                        }
                    }
                    else
                    {
                        DeckSeleccionado.Deck.Add(SearchWindow.CartaSeleccionada);
                    }
                }

                DeckSeleccionado.CheckDeckStats();
                SaveChangesDeck(DeckSeleccionado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DeckSeleccionado?.CheckDeckStats();
            }
        }

        private void DeleteMainCardAction()
        {
            try
            {
                if (DeckSeleccionado == null || SelectedCard == null)
                {
                    return;
                }

                if (MessageBox.Show($"Do you wanna remove {SelectedCard.Name} from deck?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DeckSeleccionado.Deck.Remove(SelectedCard);
                    SelectedCard = null;

                    DeckSeleccionado.CheckDeckStats();
                    SaveChangesDeck(DeckSeleccionado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DeckSeleccionado?.CheckDeckStats();
            }
        }

        #endregion

        #region Sideboard

        private async Task AddCardSideboardDeckAction()
        {
            try
            {
                if (DeckSeleccionado == null)
                {
                    return;
                }

                int MaxSideboard = 15;
                int MaxCardInside = DeckSeleccionado?.SideBoard?.Sum(k => k.Units) ?? 0;

                if (DeckSeleccionado?.SideboardCount >= 15)
                {
                    MessageBox.Show("Límite de cartas excedido. Elimine alguna carta para continuar.");
                    return;
                }

                if (API == null)
                {
                    InitiateAPI(false);
                }

                SearchCardWindow SearchWindow = new SearchCardWindow(API);
                SearchWindow.ShowDialog();

                if (SearchWindow.CartaSeleccionada != null)
                {
                    if (DeckSeleccionado.SideBoard.Any(c => c.Name == SearchWindow.CartaSeleccionada.Name))
                    {
                        CardDto Carta = DeckSeleccionado?.SideBoard.FirstOrDefault(k => k.Name == SearchWindow.CartaSeleccionada.Name);
                        if (Carta != null)
                        {
                            Carta.Units += 1;
                        }
                    }
                    else
                    {
                        DeckSeleccionado.SideBoard.Add(SearchWindow.CartaSeleccionada);
                    }
                }

                if (DeckSeleccionado != null)
                {
                    SaveChangesDeck(DeckSeleccionado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DeckSeleccionado?.CheckDeckStats();
            }
        }

        private void DeleteCardSideboardDeckAction()
        {
            try
            {
                if (DeckSeleccionado == null || SelectedSideboardCard == null)
                {
                    return;
                }

                if (MessageBox.Show($"Do you wanna remove {SelectedSideboardCard.Name} from sideboard?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DeckSeleccionado.SideBoard.Remove(SelectedSideboardCard);
                    SelectedSideboardCard = null;

                    DeckSeleccionado.CheckDeckStats();
                    SaveChangesDeck(DeckSeleccionado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DeckSeleccionado?.CheckDeckStats();
            }
        }

        #endregion

        #endregion

        #region Function
        private void SaveChangesDeck(DeckDTO SelectedDeck)
        {
            try
            {
                if (SelectedDeck != null && !string.IsNullOrWhiteSpace(SelectedDeck.DeckName))
                {
                    string JsonPath = JsonConvert.SerializeObject(SelectedDeck, Formatting.Indented);
                    if (Directory.Exists($"{FullDecksPath}\\"))
                    {
                        File.WriteAllText($"{FullDecksPath}\\{SelectedDeck.DeckName}.json", JsonPath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeleteFileDeck(DeckDTO SelectedDeck)
        {
            try
            {
                if (SelectedDeck != null && !string.IsNullOrWhiteSpace(SelectedDeck.DeckName))
                {
                    string JsonPath = JsonConvert.SerializeObject(SelectedDeck, Formatting.Indented);
                    try
                    {
                        File.Delete($"{FullDecksPath}\\{SelectedDeck.DeckName}.json");
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HideDeck(DeckDTO SelectedDeck)
        {
            try
            {
                if (SelectedDeck != null && !string.IsNullOrWhiteSpace(SelectedDeck.DeckName))
                {
                    string FilePath = $"{FullDecksPath}\\{SelectedDeck.DeckName}.json";
                    File.SetAttributes(FilePath, File.GetAttributes(FilePath) | FileAttributes.Hidden);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RecoverDeck(DeckDTO SelectedDeck)
        {
            try
            {
                if (SelectedDeck != null && !string.IsNullOrWhiteSpace(SelectedDeck.DeckName))
                {
                    SelectedDeck.DeleteDate = null;

                    string FilePath = $"{FullDecksPath}\\{SelectedDeck.DeckName}.json";
                    FileAttributes attributes = File.GetAttributes(FilePath);

                    if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        // Show the file.
                        attributes = RemoveAttribute(attributes, FileAttributes.Hidden);
                        File.SetAttributes(FilePath, attributes);
                    }

                    SaveChangesDeck(SelectedDeck);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }

        private void InitiateAPI(bool LoadSets)
        {
            try
            {
                API = new ScryfallApiClient(LoadSets);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Eventos

        // Método para manejar la acción de doble clic
        public void HandleDoubleClick(CardDto item)
        {
            try
            {

                if (item == null || (string.IsNullOrWhiteSpace(item.ImageUris.Normal?.ToString()) && item.CardFaces.Count == 0))
                {
                    return;
                }

                VisorImagesViewModel vm = new VisorImagesViewModel(item, false);
                VisorImages Visor = new VisorImages();
                Visor.DataContext = vm;
                Visor.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                Visor.Title = item.Name;
                Visor.Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
