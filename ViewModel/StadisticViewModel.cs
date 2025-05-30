using LiveCharts;
using LiveCharts.Wpf;
using MistycPawCraftCore.DTO;
using MistycPawCraftCore.Utils;
using MistycPawCraftCore.Utils.Enums;
using MistycPawCraftCore.View;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MistycPawCraftCore.ViewModel
{
    public class StadisticViewModel : INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Constructor
        public StadisticViewModel()
        {
            BasePath = System.Environment.CurrentDirectory;
            //BasePath = Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;
            FullDecksPath = $"{BasePath}\\Decks";
            DeckList = new ObservableCollection<DeckDTO>();
            IsSelectedDeck = Visibility.Collapsed;
            CheckAllSavedDecks();

            CreatureList = new ObservableCollection<CardDto>();
            PlaneswalkerList = new ObservableCollection<CardDto>();
            SpellList = new ObservableCollection<CardDto>();
            LandList = new ObservableCollection<CardDto>();
        }

        #endregion

        #region Properties

        private ObservableCollection<DeckDTO> _DeckList;
        public ObservableCollection<DeckDTO> DeckList
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

        private DeckDTO _SelectedDeck;
        public DeckDTO SelectedDeck
        {
            get
            {
                return _SelectedDeck;
            }
            set
            {
                if (value != _SelectedDeck)
                {
                    _SelectedDeck = value;
                    OnPropertyChanged("SelectedDeck");
                    CheckDeckVisibility();
                    if (SelectedDeck != null)
                    {
                        LoadLists(SelectedDeck);
                        SelectedDeck.CheckDeckStats();
                        LoadCharts(SelectedDeck);
                    }
                }

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

        private int _ArtifactsCount;
        public int ArtifactsCount
        {
            get
            {
                return _ArtifactsCount;
            }
            set
            {
                if (value != _ArtifactsCount)
                {
                    _ArtifactsCount = value;
                    OnPropertyChanged("ArtifactsCount");
                }
            }
        }

        private int _EnchantmentCount;
        public int EnchantmentCount
        {
            get
            {
                return _EnchantmentCount;
            }
            set
            {
                if (value != _EnchantmentCount)
                {
                    _EnchantmentCount = value;
                    OnPropertyChanged("EnchantmentCount");
                }
            }
        }

        #region Listas Cartas

        private ObservableCollection<CardDto> _CreatureList;
        public ObservableCollection<CardDto> CreatureList
        {
            get
            {
                return _CreatureList;
            }
            set
            {
                if (value != _CreatureList)
                {
                    _CreatureList = value;
                    OnPropertyChanged("CreatureList");
                }
            }
        }

        private CardDto _SelectedCreature;
        public CardDto SelectedCreature
        {
            get
            {
                return _SelectedCreature;
            }
            set
            {
                if (value != _SelectedCreature)
                {
                    _SelectedCreature = value;
                    OnPropertyChanged("SelectedCreature");
                }
            }
        }

        private ObservableCollection<CardDto> _PlaneswalkerList;
        public ObservableCollection<CardDto> PlaneswalkerList
        {
            get
            {
                return _PlaneswalkerList;
            }
            set
            {
                if (value != _PlaneswalkerList)
                {
                    _PlaneswalkerList = value;
                    OnPropertyChanged("PlaneswalkerList");
                }
            }
        }

        private CardDto _SelectedPlaneswalker;
        public CardDto SelectedPlaneswalker
        {
            get
            {
                return _SelectedPlaneswalker;
            }
            set
            {
                if (value != _SelectedPlaneswalker)
                {
                    _SelectedPlaneswalker = value;
                    OnPropertyChanged("SelectedPlaneswalker");
                }
            }
        }

        private ObservableCollection<CardDto> _SpellList;
        public ObservableCollection<CardDto> SpellList
        {
            get
            {
                return _SpellList;
            }
            set
            {
                if (value != _SpellList)
                {
                    _SpellList = value;
                    OnPropertyChanged("SpellList");
                }
            }
        }

        private CardDto _SelectedSpell;
        public CardDto SelectedSpell
        {
            get
            {
                return _SelectedSpell;
            }
            set
            {
                if (value != _SelectedSpell)
                {
                    _SelectedSpell = value;
                    OnPropertyChanged("SelectedSpell");
                }
            }
        }

        private ObservableCollection<CardDto> _LandList;
        public ObservableCollection<CardDto> LandList
        {
            get
            {
                return _LandList;
            }
            set
            {
                if (value != _LandList)
                {
                    _LandList = value;
                    OnPropertyChanged("LandList");
                }
            }
        }

        private CardDto _SelectedLand;
        public CardDto SelectedLand
        {
            get
            {
                return _SelectedLand;
            }
            set
            {
                if (value != _SelectedLand)
                {
                    _SelectedLand = value;
                    OnPropertyChanged("SelectedLand");
                }
            }
        }

        private ObservableCollection<CardDto> _ArtifactList;
        public ObservableCollection<CardDto> ArtifactList
        {
            get
            {
                return _ArtifactList;
            }
            set
            {
                if (value != _ArtifactList)
                {
                    _ArtifactList = value;
                    OnPropertyChanged("ArtifactList");
                }
            }
        }

        private ObservableCollection<CardDto> _EnchantmentList;
        public ObservableCollection<CardDto> EnchantmentList
        {
            get
            {
                return _EnchantmentList;
            }
            set
            {
                if (value != _EnchantmentList)
                {
                    _EnchantmentList = value;
                    OnPropertyChanged("EnchantmentList");
                }
            }
        }

        #endregion

        #region Visibility

        private Visibility _CreatureVisibility;
        public Visibility CreatureVisibility
        {
            get
            {
                return _CreatureVisibility;
            }
            set
            {
                if (value != _CreatureVisibility)
                {
                    _CreatureVisibility = value;
                    OnPropertyChanged("CreatureVisibility");
                }
            }
        }

        private Visibility _LandsVisibility;
        public Visibility LandsVisibility
        {
            get
            {
                return _LandsVisibility;
            }
            set
            {
                if (value != _LandsVisibility)
                {
                    _LandsVisibility = value;
                    OnPropertyChanged("LandsVisibility");
                }
            }
        }

        private Visibility _SpellsVisibility;
        public Visibility SpellsVisibility
        {
            get
            {
                return _SpellsVisibility;
            }
            set
            {
                if (value != _SpellsVisibility)
                {
                    _SpellsVisibility = value;
                    OnPropertyChanged("SpellsVisibility");
                }
            }
        }

        private Visibility _PlanesWalkerVisibility;
        public Visibility PlanesWalkerVisibility
        {
            get
            {
                return _PlanesWalkerVisibility;
            }
            set
            {
                if (value != _PlanesWalkerVisibility)
                {
                    _PlanesWalkerVisibility = value;
                    OnPropertyChanged("PlanesWalkerVisibility");
                }
            }
        }

        private Visibility _ArtifactsVisibility;
        public Visibility ArtifactsVisibility
        {
            get
            {
                return _ArtifactsVisibility;
            }
            set
            {
                if (value != _ArtifactsVisibility)
                {
                    _ArtifactsVisibility = value;
                    OnPropertyChanged("ArtifactsVisibility");
                }
            }
        }

        private Visibility _EnchantmentVisibility;
        public Visibility EnchantmentVisibility
        {
            get
            {
                return _EnchantmentVisibility;
            }
            set
            {
                if (value != _EnchantmentVisibility)
                {
                    _EnchantmentVisibility = value;
                    OnPropertyChanged("EnchantmentVisibility");
                }
            }
        }

        #endregion

        #region Graphics

        private SeriesCollection _LineSeriesCollection;
        public SeriesCollection LineSeriesCollection
        {
            get
            {
                return _LineSeriesCollection;
            }
            set
            {
                if (value != _LineSeriesCollection)
                {
                    _LineSeriesCollection = value;
                    OnPropertyChanged("LineSeriesCollection");
                }
            }
        }

        private SeriesCollection _AdvancedSeries;
        public SeriesCollection AdvancedSeries
        {
            get
            {
                return _AdvancedSeries;
            }
            set
            {
                if (value != _AdvancedSeries)
                {
                    _AdvancedSeries = value;
                    OnPropertyChanged("AdvancedSeries");
                }
            }
        }

        #endregion

        private PieSeries _SelectedPie;
        public PieSeries SelectedPie
        {
            get
            {
                return _SelectedPie;
            }
            set
            {
                if (value != _SelectedPie)
                {
                    _SelectedPie = value;
                    OnPropertyChanged("SelectedPie");
                }
            }
        }

        #endregion

        #region Commands

        private ICommand _RefreshDecksCommand;
        public ICommand RefreshDecksCommand
        {
            get
            {
                _RefreshDecksCommand = new CommandHandler(() => CheckAllSavedDecks(), true);
                return _RefreshDecksCommand;
            }
        }

        #endregion

        #region Actions


        #endregion

        #region Events

        public string BasePath { get; set; }
        public string FullDecksPath { get; set; }
        private void CheckAllSavedDecks()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullDecksPath))
                {
                    return;
                }

                DeckList.Clear();
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
                        DeckList.Add(DeckLeido);
                    }
                }

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
                if (SelectedDeck == null)
                {
                    IsSelectedDeck = Visibility.Collapsed;
                }
                else
                {
                    IsSelectedDeck = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadLists(DeckDTO Deck)
        {
            try
            {
                ArtifactsCount = 0;
                EnchantmentCount = 0;

                if (Deck == null || string.IsNullOrWhiteSpace(Deck.DeckName))
                {
                    return;
                }

                if (CreatureList == null)
                {
                    CreatureList = new ObservableCollection<CardDto>();
                }
                else
                {
                    CreatureList.Clear();
                }

                if (SpellList == null)
                {
                    SpellList = new ObservableCollection<CardDto>();
                }
                else
                {
                    SpellList.Clear();
                }

                if (LandList == null)
                {
                    LandList = new ObservableCollection<CardDto>();
                }
                else
                {
                    LandList.Clear();
                }

                if (PlaneswalkerList == null)
                {
                    PlaneswalkerList = new ObservableCollection<CardDto>();
                }
                else
                {
                    PlaneswalkerList.Clear();
                }

                if (ArtifactList == null)
                {
                    ArtifactList = new ObservableCollection<CardDto>();
                }
                else
                {
                    ArtifactList.Clear();
                }

                if (EnchantmentList == null)
                {
                    EnchantmentList = new ObservableCollection<CardDto>();
                }
                else
                {
                    EnchantmentList.Clear();
                }

                foreach (CardDto card in Deck.Deck)
                {
                    if (string.IsNullOrWhiteSpace(card.TypeAndClass?.SuperType?.FullSuperType))
                    {
                        continue;
                    }

                    if (card.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Creature.ToString()))
                    {
                        CreatureList.Add(card);
                    }
                    else if (card.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Sorcery.ToString()))
                    {
                        SpellList.Add(card);
                    }
                    else if (card.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Instant.ToString()))
                    {
                        SpellList.Add(card);
                    }

                    else if (card.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Enchantment.ToString()))
                    {
                        EnchantmentList.Add(card);
                        EnchantmentCount = EnchantmentCount + 1;
                    }
                    else if (card.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Artifact.ToString()))
                    {
                        ArtifactList.Add(card);
                        ArtifactList.ToList().ForEach(k => ArtifactsCount = ArtifactsCount + k.Units);
                    }

                    else if (card.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Planeswalker.ToString()))
                    {
                        PlaneswalkerList.Add(card);
                    }
                    else if (card.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Land.ToString()))
                    {
                        LandList.Add(card);
                    }

                }

                LoadVisibilityLists();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadVisibilityLists()
        {
            try
            {
                CreatureVisibility = (CreatureList == null | CreatureList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
                SpellsVisibility = (SpellList == null | SpellList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
                PlanesWalkerVisibility = (PlaneswalkerList == null | PlaneswalkerList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
                LandsVisibility = (LandList == null | LandList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
                ArtifactsVisibility = (ArtifactList == null | ArtifactList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
                EnchantmentVisibility = (EnchantmentList == null | EnchantmentList.Count == 0) ? Visibility.Collapsed : Visibility.Visible;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadCharts(DeckDTO selectedDeck)
        {
            try
            {
                LineSeriesCollection = new SeriesCollection();

                #region Top Chart

                if (selectedDeck.PlaneswalkerCount > 0)
                {
                    LineSeriesCollection.Add(new PieSeries
                    {
                        Title = MtgCardType.Planeswalker.ToString(),
                        Values = new ChartValues<double> { selectedDeck.PlaneswalkerCount },
                        DataLabels = true,
                        Fill = new SolidColorBrush(Colors.Gray),
                    });
                }

                if (selectedDeck.TotalNonCreatureAndInstantSpells > 0)
                {
                    LineSeriesCollection.Add(new PieSeries
                    {
                        Title = "Spells",
                        Values = new ChartValues<double> { selectedDeck.TotalNonCreatureAndInstantSpells },
                        DataLabels = true,
                        Fill = new SolidColorBrush(Colors.Blue)

                    });
                }

                if (selectedDeck.CreatureCount > 0)
                {
                    LineSeriesCollection.Add(new PieSeries
                    {
                        Title = MtgCardType.Creature.ToString(),
                        Values = new ChartValues<double> { selectedDeck.CreatureCount },
                        DataLabels = true,
                        Fill = new SolidColorBrush(Colors.Red)

                    });
                }

                if (selectedDeck.LandCount > 0)
                {
                    LineSeriesCollection.Add(new PieSeries
                    {
                        Title = MtgCardType.Land.ToString(),
                        Values = new ChartValues<double> { selectedDeck.LandCount },
                        DataLabels = true,
                        Fill = new SolidColorBrush(Colors.ForestGreen),

                    });
                }

                if (ArtifactList.Count > 0)
                {
                    LineSeriesCollection.Add(new PieSeries
                    {
                        Title = MtgCardType.Artifact.ToString(),
                        Values = new ChartValues<double> { ArtifactsCount },
                        DataLabels = true,
                        Fill = new SolidColorBrush(Colors.LightGray)

                    });
                }

                if (EnchantmentList.Count > 0)
                {
                    int EnchantmentConut = selectedDeck.Deck.Where(k => k.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Enchantment.ToString().ToLower())).Count();
                    LineSeriesCollection.Add(new PieSeries
                    {
                        Title = MtgCardType.Enchantment.ToString(),
                        Values = new ChartValues<double> { EnchantmentCount },
                        DataLabels = true,
                        Fill = new SolidColorBrush(Colors.LightCoral)

                    });
                }

                #endregion

                AdvancedSeries = new SeriesCollection();

                #region Bottom Chart

                ChartValues<double> SharedValues = new ChartValues<double>();

                if (PlaneswalkerList.Count > 0)
                {
                    SharedValues = new ChartValues<double>();

                    for (int i = 0; i <= 10; i++)
                    {
                        int count = 0;

                        foreach (CardDto Card in PlaneswalkerList.Where(k => k.CMC == i))
                        {
                            count = count + Card.Units;
                        }
                        SharedValues.Add(count);
                    }

                    AdvancedSeries.Add(new ColumnSeries
                    {
                        Title = MtgCardType.Planeswalker.ToString(),
                        Values = SharedValues,
                        DataLabels = true,
                        ToolTip = "Planeswalker/s",
                        //Foreground = (Properties.Settings.Default.DarkMode) ?
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.WhiteSmoke.A, System.Drawing.Color.WhiteSmoke.R, System.Drawing.Color.WhiteSmoke.G, System.Drawing.Color.WhiteSmoke.B)) :
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.Black.A, System.Drawing.Color.Black.R, System.Drawing.Color.Black.G, System.Drawing.Color.Black.B)),
                        Visibility = PlaneswalkerList.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                        Fill = new SolidColorBrush(Colors.Gray),
                    });
                }

                if (SpellList.Count > 0)
                {
                    SharedValues = new ChartValues<double>();

                    for (int i = 0; i <= 10; i++)
                    {
                        int count = 0;

                        foreach (CardDto Card in SpellList.Where(k => k.CMC == i))
                        {
                            count = count + Card.Units;
                        }
                        SharedValues.Add(count);
                    }

                    AdvancedSeries.Add(new ColumnSeries
                    {
                        Title = "Spells",
                        Values = SharedValues,
                        ToolTip = "Spell/s",
                        DataLabels = true,
                        Visibility = SpellList.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                        //Foreground = (Properties.Settings.Default.DarkMode) ?
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.WhiteSmoke.A, System.Drawing.Color.WhiteSmoke.R, System.Drawing.Color.WhiteSmoke.G, System.Drawing.Color.WhiteSmoke.B)) :
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.Black.A, System.Drawing.Color.Black.R, System.Drawing.Color.Black.G, System.Drawing.Color.Black.B)),
                        Fill = new SolidColorBrush(Colors.Blue)

                    });
                }

                if (CreatureList.Count > 0)
                {
                    SharedValues = new ChartValues<double>();

                    for (int i = 0; i <= 10; i++)
                    {
                        int count = 0;

                        foreach (CardDto Card in CreatureList.Where(k => k.CMC == i))
                        {
                            count = count + Card.Units;
                        }
                        SharedValues.Add(count);
                    }

                    AdvancedSeries.Add(new ColumnSeries
                    {
                        Title = MtgCardType.Creature.ToString(),
                        Values = SharedValues,
                        ToolTip = "Creature/s",
                        DataLabels = true,
                        Visibility = CreatureList.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                        //Foreground = (Properties.Settings.Default.DarkMode) ?
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.WhiteSmoke.A, System.Drawing.Color.WhiteSmoke.R, System.Drawing.Color.WhiteSmoke.G, System.Drawing.Color.WhiteSmoke.B)) :
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.Black.A, System.Drawing.Color.Black.R, System.Drawing.Color.Black.G, System.Drawing.Color.Black.B)),
                        Fill = new SolidColorBrush(Colors.Red)

                    });
                }

                if (LandList.Count > 0)
                {
                    AdvancedSeries.Add(new ColumnSeries
                    {
                        Title = MtgCardType.Land.ToString(),
                        Values = new ChartValues<double> { selectedDeck.LandCount },
                        ToolTip = "Land/s",
                        DataLabels = true,
                        Visibility = LandList.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                        //Foreground = (Properties.Settings.Default.DarkMode) ?
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.WhiteSmoke.A, System.Drawing.Color.WhiteSmoke.R, System.Drawing.Color.WhiteSmoke.G, System.Drawing.Color.WhiteSmoke.B)) :
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.Black.A, System.Drawing.Color.Black.R, System.Drawing.Color.Black.G, System.Drawing.Color.Black.B)),
                        Fill = new SolidColorBrush(Colors.LightGreen)

                    });
                }

                if (ArtifactList.Count > 0)
                {
                    SharedValues = new ChartValues<double>();

                    for (int i = 0; i <= 10; i++)
                    {
                        int count = 0;

                        foreach (CardDto Card in ArtifactList.Where(k => k.CMC == i))
                        {
                            count = count + Card.Units;
                        }

                        SharedValues.Add(count);
                    }

                    AdvancedSeries.Add(new ColumnSeries
                    {
                        Title = MtgCardType.Artifact.ToString(),
                        Values = SharedValues,
                        ToolTip = "Artifact/s",
                        DataLabels = true,
                        ScalesXAt = 0,
                        Visibility = ArtifactList.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                        //Foreground = (Properties.Settings.Default.DarkMode) ?
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.WhiteSmoke.A, System.Drawing.Color.WhiteSmoke.R, System.Drawing.Color.WhiteSmoke.G, System.Drawing.Color.WhiteSmoke.B)) :
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.Black.A, System.Drawing.Color.Black.R, System.Drawing.Color.Black.G, System.Drawing.Color.Black.B)),
                        Fill = new SolidColorBrush(Colors.LightGray)

                    });
                }

                if (EnchantmentList.Count > 0)
                {
                    SharedValues = new ChartValues<double>();

                    for (int i = 0; i <= 10; i++)
                    {
                        int count = 0;

                        foreach (CardDto Card in EnchantmentList.Where(k => k.CMC == i))
                        {
                            count = count + Card.Units;
                        }

                        SharedValues.Add(count);
                    }

                    AdvancedSeries.Add(new ColumnSeries
                    {
                        Title = MtgCardType.Enchantment.ToString(),
                        Values = SharedValues,
                        ToolTip = "Enchantment/s",
                        DataLabels = true,
                        ScalesXAt = 0,
                        Visibility = EnchantmentList.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                        //Foreground = (Properties.Settings.Default.DarkMode) ?
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.WhiteSmoke.A, System.Drawing.Color.WhiteSmoke.R, System.Drawing.Color.WhiteSmoke.G, System.Drawing.Color.WhiteSmoke.B)) :
                        //                new SolidColorBrush(Color.FromArgb(System.Drawing.Color.Black.A, System.Drawing.Color.Black.R, System.Drawing.Color.Black.G, System.Drawing.Color.Black.B)),
                        Fill = new SolidColorBrush(Colors.LightCoral)

                    });
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void SelectPie(ChartPoint chartPoint)
        {
            try
            {
                SelectedPie = chartPoint.SeriesView as PieSeries;

                if (SelectedPie != null)
                {
                    if (SelectedPie.PushOut != 0)
                    {
                        LineSeriesCollection.OfType<PieSeries>().ToList().ForEach(k => k.PushOut = 0);
                        AdvancedSeries.OfType<ColumnSeries>().ToList().ForEach(k => k.Visibility = Visibility.Visible);
                    }
                    else
                    {
                        foreach (PieSeries series in LineSeriesCollection.OfType<PieSeries>())
                        {
                            series.PushOut = SelectedPie.Title == series.Title ? 15 : 0;
                        }

                        foreach (ColumnSeries advanceserie in AdvancedSeries.OfType<ColumnSeries>())
                        {
                            advanceserie.Visibility = SelectedPie.Title == advanceserie.Title ? Visibility.Visible : Visibility.Collapsed;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Método para manejar la acción de doble clic
        public void HandleDoubleClick(CardDto item)
        {
            try
            {

                if (item == null || string.IsNullOrWhiteSpace(item.ImageUris.Normal?.ToString()))
                {
                    return;
                }

                VisorImages Visor = new VisorImages(item.ImageUris.Normal.ToString());
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
