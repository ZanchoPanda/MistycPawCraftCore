using Microsoft.Win32;
using MistycPawCraftCore.DTO;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MistycPawCraftCore.ViewModel
{
    public class ImportViewModel : INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public event EventHandler RequestClose;

        #endregion

        #region Properties

        private string _FilePath;
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                if (value != _FilePath)
                {
                    _FilePath = value;
                    OnPropertyChanged("FilePath");
                }
            }
        }

        private string _DeckList;
        public string DeckList
        {
            get
            {
                return _DeckList;
            }
            set
            {
                if (value != _DeckList)
                {
                    _DeckList = value;
                    OnPropertyChanged("DeckList");
                }
            }
        }

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

        private string _BasePath;
        public string BasePath
        {
            get
            {
                return _BasePath;
            }
            set
            {
                if (value != _BasePath)
                {
                    _BasePath = value;
                    OnPropertyChanged("BasePath");
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

        private string _HelperText;
        public string HelperText
        {
            get
            {
                return _HelperText;
            }
            set
            {
                if (value != _HelperText)
                {
                    _HelperText = value;
                    OnPropertyChanged("HelperText");
                }
            }
        }

        #endregion

        #region Constructor

        public ImportViewModel()
        {

            InitiateAPI(false);
            BasePath = System.Environment.CurrentDirectory;
            //BasePath = Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;
            FullDecksPath = $"{BasePath}\\Decks";
            Directory.CreateDirectory(FullDecksPath);
            HelperText = $"2 Opt (2x2){Environment.NewLine}15 Mountain (fin){Environment.NewLine}2 Jace Beleren (ixl){Environment.NewLine}{Environment.NewLine}Sideboard{Environment.NewLine}2 Opt{Environment.NewLine}2 Counterspell";
        }

        #endregion

        #region Commands + Actions

        private ICommand _SearchFileCommand;
        public ICommand SearchFileCommand
        {
            get
            {
                _SearchFileCommand = new CommandHandler(() => SearchFileAction(), true);
                return _SearchFileCommand;
            }
        }

        StringBuilder StringDeck = new StringBuilder();

        private void SearchFileAction()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Cards not founded or wrong tiped:");
                FilePath = string.Empty;

                OpenFileDialog OFileDialog = new OpenFileDialog();
                OFileDialog.InitialDirectory = "c:\\";
                OFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                OFileDialog.FilterIndex = 2;
                OFileDialog.RestoreDirectory = true;

                if (OFileDialog.ShowDialog().Value)
                {
                    FilePath = OFileDialog.FileName;
                }

                if (!string.IsNullOrWhiteSpace(FilePath))
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    DeckList = string.Empty;

                    List<CardDto> Cards = new List<CardDto>();
                    try
                    {
                        StringDeck = new StringBuilder();

                        string[] lines = File.ReadAllLines(FilePath);
                        foreach (string Line in lines)
                        {
                            if (Line == "Main" | Line == "//Main" | Line == "Deck" | Line == "//Deck")
                            {
                                StringDeck.AppendLine("Main");
                                continue;
                            }

                            if (Line == "Sideboard" | Line == "//Sideboard")
                            {
                                StringDeck.AppendLine("Sideboard");
                                continue;
                            }

                            string[] Parts = Line.Split(' ');

                            if (Parts.Length >= 2)
                            {
                                int quantity = 0;
                                if (Parts[0].All(char.IsDigit))
                                {
                                    int.TryParse(Parts[0], out quantity);
                                }
                                else
                                {
                                    string numberOnly = Regex.Replace(Parts[0], "[^0-9.]", "");
                                    int.TryParse(numberOnly, out quantity);
                                }

                                if (quantity != 0)
                                {
                                    string name = string.Join(" ", Parts.Skip(1));

                                    StringDeck.AppendLine($"{quantity} {name}");
                                }
                            }

                        }

                        Mouse.OverrideCursor = null;

                    }
                    catch (Exception exp)
                    {
                        Mouse.OverrideCursor = null;
                        string mensaje = LocalizationManager.GetString("FileWithErrors");
                        CustomDialog cd = new CustomDialog(Utils.Enums.EnumTitleMessage.Error, mensaje, Utils.Enums.EnumButtonsMessage.Acept);
                        cd.ShowInTaskbar = true;
                        cd.ShowDialog();
                        StringDeck = (StringDeck != null) ? StringDeck.Clear() : new StringBuilder();
                    }

                    if (Regex.Matches(sb.ToString(), Environment.NewLine).Count > 1)
                    {
                        MessageBox.Show(sb.ToString());
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DeckList = StringDeck.ToString();
            }
        }

        private ICommand _ClearFilePathCommand;
        public ICommand ClearFilePathCommand
        {
            get
            {
                _ClearFilePathCommand = new CommandHandler(() => ClearFilePathAction(), true);
                return _ClearFilePathCommand;
            }
        }

        private void ClearFilePathAction()
        {
            try
            {
                FilePath = string.Empty;
                DeckList = string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ICommand _AcceptCommand;
        public ICommand AcceptCommand
        {
            get
            {
                _AcceptCommand = new CommandHandler(() => AcceptActionAsync(), true);
                return _AcceptCommand;
            }
        }

        private async Task AcceptActionAsync()
        {
            try
            {
                string mensaje = string.Empty;
                if (string.IsNullOrWhiteSpace(DeckList))
                {
                    mensaje = LocalizationManager.GetString("NothingAcceptedToImport");
                    CustomDialog cd = new CustomDialog(Utils.Enums.EnumTitleMessage.Info, mensaje, Utils.Enums.EnumButtonsMessage.Acept);
                    cd.ShowInTaskbar = true;
                    cd.ShowDialog();
                }
                else
                {

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(LocalizationManager.GetString("CardsNotNoundedWrongTiped"));
                    try
                    {
                        string[] DeckListArray = DeckList.Split(new char[] { '\r', '\n' });

                        DeckDTO ImportedDeck = new DeckDTO();

                        if (string.IsNullOrWhiteSpace(FilePath))
                        {
                            DeckNameView Vista = new DeckNameView();
                            Vista.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            Vista.ShowDialog();
                            if (Vista.NombreAceptado)
                            {
                                ImportedDeck.DeckName = Vista.DeckName;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            ImportedDeck.DeckName = Path.GetFileNameWithoutExtension(FilePath);
                        }

                        Mouse.OverrideCursor = Cursors.Wait;
                        bool isMainDeck = true;
                        foreach (string Line in DeckListArray)
                        {
                            if (string.IsNullOrWhiteSpace(Line))
                            {
                                continue;
                            }
                            if (Line == "Main" | Line == "Deck" | Line == "//Main" | Line == "//Deck")
                            {
                                isMainDeck = true;
                                ImportedDeck.Deck = ImportedDeck.Deck == null ? new ObservableCollection<CardDto>() : ImportedDeck.Deck;
                                continue;
                            }

                            if (Line == "Sideboard" | Line == "//Sideboard")
                            {
                                isMainDeck = false;
                                ImportedDeck.SideBoard = ImportedDeck.SideBoard == null ? new ObservableCollection<CardDto>() : ImportedDeck.SideBoard;
                                continue;
                            }

                            string[] Parts = Line.Split(' ');

                            if (Parts.Length >= 2)
                            {
                                int quantity = 0;
                                if (Parts[0].All(char.IsDigit))
                                {
                                    int.TryParse(Parts[0], out quantity);
                                }
                                else
                                {
                                    string numberOnly = Regex.Replace(Parts[0], "[^0-9.]", "");
                                    int.TryParse(numberOnly, out quantity);
                                }

                                if (quantity != 0)
                                {
                                    string name = string.Join(" ", Parts.Skip(1));

                                    string? setCode = null;
                                    // Detectar si hay un set entre paréntesis al final (ej: Lightning Bolt (2XM))
                                    Match setMatch = Regex.Match(name, @"\(([^)]+)\)$");
                                    if (setMatch.Success)
                                    {
                                        setCode = setMatch.Groups[1].Value.Trim();
                                        name = name.Substring(0, setMatch.Index).Trim();
                                    }

                                    name = API.CleanNameCard(name).Trim();

                                    List<CardDto> SearchedCard = await API.GetCardByName(name, setCode);

                                    if (SearchedCard != null && SearchedCard.Count > 0)
                                    {
                                        CardDto card = SearchedCard.FirstOrDefault();
                                        card.Units = quantity;

                                        if (isMainDeck)
                                        {
                                            ImportedDeck.Deck.Add(card);
                                        }
                                        else
                                        {
                                            ImportedDeck.SideBoard.Add(card);
                                        }

                                    }
                                    else
                                    {
                                        sb.AppendLine($"{name}");
                                    }
                                }
                            }
                        }

                        if (ImportedDeck.Deck.Count == 0 && ImportedDeck.SideBoard.Count == 0)
                        {
                            mensaje = LocalizationManager.GetString("ErrorLoadCard");
                            CustomDialog cdImported = new CustomDialog(EnumTitleMessage.Info, mensaje, EnumButtonsMessage.Acept);
                            cdImported.ShowDialog();
                            return;
                            RequestClose?.Invoke(this, EventArgs.Empty);
                        }

                        ImportedDeck.CheckDeckStats();
                        SaveChangesDeck(ImportedDeck);

                        Mouse.OverrideCursor = null;
                        mensaje = LocalizationManager.GetString("DeckImportedOK");
                        CustomDialog cd = new CustomDialog(Utils.Enums.EnumTitleMessage.Info, mensaje, Utils.Enums.EnumButtonsMessage.Acept);
                        cd.ShowDialog();

                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    Mouse.OverrideCursor = null;
                    if (Regex.Matches(sb.ToString(), Environment.NewLine).Count > 1)
                    {
                        CustomDialog cd = new CustomDialog(Utils.Enums.EnumTitleMessage.Warning, sb.ToString(), Utils.Enums.EnumButtonsMessage.Acept);
                        cd.ShowDialog();
                    }

                    RequestClose?.Invoke(this, EventArgs.Empty);
                    //CancelAction();
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


        private ICommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                _CancelCommand = new CommandHandler(() => CancelAction(), true);
                return _CancelCommand;
            }
        }

        public void CancelAction()
        {
            try
            {
                DeckList = string.Empty;
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Others

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

        private void SaveChangesDeck(DeckDTO SelectedDeck)
        {
            try
            {
                if (SelectedDeck != null && !string.IsNullOrWhiteSpace(SelectedDeck.DeckName))
                {
                    string JsonPath = JsonConvert.SerializeObject(SelectedDeck, Formatting.Indented);
                    if (Directory.Exists($"{FullDecksPath}\\"))
                    {
                        if (File.Exists($"{FullDecksPath}\\{SelectedDeck.DeckName}.json"))
                        {
                            File.Delete($"{FullDecksPath}\\{SelectedDeck.DeckName}.json");
                        }

                        File.WriteAllText($"{FullDecksPath}\\{SelectedDeck.DeckName}.json", JsonPath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
