using MistycPawCraftCore.Utils.Enums;
using MistycPawCraftCore.Utils.Language;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MistycPawCraftCore.DTO
{
    public class DeckDTO : INotifyPropertyChanged
    {
        #region Event
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Constructor

        public DeckDTO()
        {
            try
            {

                Legal = new LegalitiesDto();
                Deck = new ObservableCollection<CardDto>();
                SideBoard = new ObservableCollection<CardDto>();

            }
            catch (Exception ex)
            {
                var aux = ex;
            }
        }

        #endregion

        #region Properties

        private int _IDDeck;
        public int IDDeck
        {
            get
            {
                return _IDDeck;
            }
            set
            {
                if (value != _IDDeck)
                {
                    _IDDeck = value;
                    OnPropertyChanged("IDDeck");
                }
            }
        }

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

        private LegalitiesDto _Legal;
        public LegalitiesDto Legal
        {
            get
            {
                return _Legal;
            }
            set
            {
                if (value != _Legal)
                {
                    _Legal = value;
                    OnPropertyChanged("Legal");
                }
            }
        }

        private ObservableCollection<CardDto> _Deck;
        public ObservableCollection<CardDto> Deck
        {
            get
            {
                return _Deck;
            }
            set
            {
                if (value != _Deck)
                {
                    _Deck = value;
                    OnPropertyChanged("Deck");
                }
            }
        }

        private ObservableCollection<CardDto> _SideBoard;
        public ObservableCollection<CardDto> SideBoard
        {
            get
            {
                return _SideBoard;
            }
            set
            {
                if (value != _SideBoard)
                {
                    _SideBoard = value;
                    OnPropertyChanged("SideBoard");
                }
            }
        }

        private int _SideboardCount;
        public int SideboardCount
        {
            get
            {
                return _SideboardCount;
            }
            set
            {
                if (value != _SideboardCount)
                {
                    _SideboardCount = value;
                    OnPropertyChanged("SideboardCount");
                }
            }
        }

        #region Estadistics

        private int _TotalMainCards;
        public int TotalMainCards
        {
            get
            {
                return _TotalMainCards;
            }
            set
            {
                if (value != _TotalMainCards)
                {
                    _TotalMainCards = value;
                    OnPropertyChanged("TotalMainCards");
                }
            }
        }

        private int _CreatureCount;
        public int CreatureCount
        {
            get
            {
                return _CreatureCount;
            }
            set
            {
                if (value != _CreatureCount)
                {
                    _CreatureCount = value;
                    OnPropertyChanged("CreatureCount");
                }
            }
        }

        private int _SorceryAndInstantsCount;
        public int SorceryAndInstantsCount
        {
            get
            {
                return _SorceryAndInstantsCount;
            }
            set
            {
                if (value != _SorceryAndInstantsCount)
                {
                    _SorceryAndInstantsCount = value;
                    OnPropertyChanged("SorceryAndInstantsCount");
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

        private int _PlaneswalkerCount;
        public int PlaneswalkerCount
        {
            get
            {
                return _PlaneswalkerCount;
            }
            set
            {
                if (value != _PlaneswalkerCount)
                {
                    _PlaneswalkerCount = value;
                    OnPropertyChanged("PlaneswalkerCount");
                }
            }
        }

        private int _LandCount;
        public int LandCount
        {
            get
            {
                return _LandCount;
            }
            set
            {
                if (value != _LandCount)
                {
                    _LandCount = value;
                    OnPropertyChanged("LandCount");
                }
            }
        }

        private int _TotalNonCreatureAndInstantSpells;
        public int TotalNonCreatureAndInstantSpells
        {
            get
            {
                return _TotalNonCreatureAndInstantSpells;
            }
            set
            {
                if (value != _TotalNonCreatureAndInstantSpells)
                {
                    _TotalNonCreatureAndInstantSpells = value;
                    OnPropertyChanged("TotalNonCreatureAndInstantSpells");
                }
            }
        }

        private decimal _TotalMainDeckPrice;
        public decimal TotalMainDeckPrice
        {
            get
            {
                return _TotalMainDeckPrice;
            }
            set
            {
                if (value != _TotalMainDeckPrice)
                {
                    _TotalMainDeckPrice = value;
                    OnPropertyChanged("TotalMainDeckPrice");
                }
            }
        }

        private decimal _TotalSideboardPrice;
        public decimal TotalSideboardPrice
        {
            get
            {
                return _TotalSideboardPrice;
            }
            set
            {
                if (value != _TotalSideboardPrice)
                {
                    _TotalSideboardPrice = value;
                    OnPropertyChanged("TotalSideboardPrice");
                }
            }
        }

        private decimal _TotalDeckPrice;
        public decimal TotalDeckPrice
        {
            get
            {
                return _TotalDeckPrice;
            }
            set
            {
                if (value != _TotalDeckPrice)
                {
                    _TotalDeckPrice = value;
                    OnPropertyChanged("TotalDeckPrice");
                }
            }
        }

        #endregion

        #region Control

        private DateTime? _CreateDate;
        public DateTime? CreateDate
        {
            get
            {
                return _CreateDate;
            }
            set
            {
                if (value != _CreateDate)
                {
                    _CreateDate = value;
                    OnPropertyChanged("CreateDate");
                }
            }
        }

        private DateTime? _DeleteDate;
        public DateTime? DeleteDate
        {
            get
            {
                return _DeleteDate;
            }
            set
            {
                if (value != _DeleteDate)
                {
                    _DeleteDate = value;
                    OnPropertyChanged("DeleteDate");
                }
            }
        }

        #endregion

        #endregion

        #region Functions

        private void CheckCardsTypes(object sender, PropertyChangedEventArgs e)
        {
            try
            {



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CheckDeckStats()
        {
            try
            {

                if (Deck != null && Deck.Count > 0)
                {
                    TotalMainCards = 0;
                    CreatureCount = 0;
                    EnchantmentCount = 0;
                    SorceryAndInstantsCount = 0;
                    TotalNonCreatureAndInstantSpells = 0;
                    PlaneswalkerCount = 0;
                    LandCount = 0;
                    SideboardCount = 0;

                    Deck.ToList().ForEach(k => TotalMainCards = TotalMainCards + k.Units);
                    SideBoard.ToList().ForEach(k => SideboardCount = SideboardCount + k.Units);

                    foreach (CardDto Card in Deck.Where(k => !string.IsNullOrWhiteSpace(k.TypeAndClass.SuperType.FullSuperType) && k.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Creature.ToString())))
                    {
                        CreatureCount = CreatureCount + Card.Units;
                    }

                    foreach (CardDto Card in Deck.Where(k => !string.IsNullOrWhiteSpace(k.TypeAndClass.SuperType.FullSuperType) && k.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Enchantment.ToString())))
                    {
                        EnchantmentCount = EnchantmentCount + Card.Units;
                    }

                    foreach (CardDto Card in Deck.Where(k => !string.IsNullOrWhiteSpace(k.TypeAndClass.SuperType.FullSuperType) && k.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Sorcery.ToString()) || k.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Instant.ToString())))
                    {
                        SorceryAndInstantsCount = SorceryAndInstantsCount + Card.Units;
                    }

                    foreach (CardDto Card in Deck.Where(k => !string.IsNullOrWhiteSpace(k.TypeAndClass.SuperType.FullSuperType) && k.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Planeswalker.ToString())))
                    {
                        PlaneswalkerCount = PlaneswalkerCount + Card.Units;
                    }

                    foreach (CardDto Card in Deck.Where(k => !string.IsNullOrWhiteSpace(k.TypeAndClass.SuperType.FullSuperType) && k.TypeAndClass.SuperType.FullSuperType.Contains(MtgCardType.Land.ToString())))
                    {
                        LandCount = LandCount + Card.Units;
                    }

                    foreach (CardDto card in Deck.ToList())
                    {
                        card.TotalPriceCardMarket = card.CardMarketPrice.eur * card.Units;
                    }

                    foreach (CardDto card in SideBoard)
                    {
                        card.TotalPriceCardMarket = card.CardMarketPrice.eur * card.Units;
                    }

                    TotalNonCreatureAndInstantSpells = SorceryAndInstantsCount + EnchantmentCount;

                    CalculatePriceDeck();
                    CheckLegalitiesDeck();
                    CheckMaximunUnits();

                    #region MyRegion
                    //Legal.Commander = (Deck.Any(k => k.Legalities.Commander == false) | SideBoard.Any(k => k.Legalities.Commander == false)) ? false : true;
                    //Legal.Historic = (Deck.Any(k => k.Legalities.Historic == false) | SideBoard.Any(k => k.Legalities.Historic == false)) ? false : true;
                    //Legal.Legacy = (Deck.Any(k => k.Legalities.Legacy == false) | SideBoard.Any(k => k.Legalities.Legacy == false)) ? false : true;
                    //Legal.Modern = (Deck.Any(k => k.Legalities.Modern == false) | SideBoard.Any(k => k.Legalities.Modern == false)) ? false : true;
                    //Legal.Pauper = (Deck.Any(k => k.Legalities.Pauper == false) | SideBoard.Any(k => k.Legalities.Pauper == false)) ? false : true;
                    //Legal.Pioneer = (Deck.Any(k => k.Legalities.Pioneer == false) | SideBoard.Any(k => k.Legalities.Pioneer == false)) ? false : true;
                    //Legal.Standard = (Deck.Any(k => k.Legalities.Standard == false) | SideBoard.Any(k => k.Legalities.Standard == false)) ? false : true;
                    #endregion

                }

            }
            catch (Exception ex)
            {
                var aux = ex;
            }
        }

        public void CalculatePriceDeck()
        {
            try
            {
                TotalMainDeckPrice = 0.0m;
                TotalSideboardPrice = 0.0m;
                TotalDeckPrice = 0.0m;

                Deck.ToList().ForEach(k => TotalMainDeckPrice = TotalMainDeckPrice + k.TotalPriceCardMarket);
                SideBoard.ToList().ForEach(k => TotalSideboardPrice = TotalSideboardPrice + k.TotalPriceCardMarket);

                TotalDeckPrice = TotalMainDeckPrice + TotalSideboardPrice;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckLegalitiesDeck()
        {
            try
            {

                if (Deck != null && SideBoard != null)
                {
                    string landkeyword = LocalizationManager.GetString("MtgCardType_Land").ToLower();

                    if (Deck.Any(k => !k.Legalities.Commander || Deck.Any(k => !string.IsNullOrWhiteSpace(k.TypeAndClass.CardType.FullCardType) &&
                                        !k.TypeAndClass.SuperType.FullSuperType.ToLower().Contains(landkeyword) && k.Units > 1))

                        || SideBoard.Any(k => !k.Legalities.Commander || SideBoard.Any(k => !string.IsNullOrWhiteSpace(k.TypeAndClass.CardType.FullCardType) &&
                                          !k.TypeAndClass.SuperType.FullSuperType.ToLower().Contains(landkeyword) && k.Units > 1)))
                    {
                        Legal.Commander = false;
                    }
                    else
                    {
                        Legal.Commander = true;
                    }

                    if (Deck.Any(k => !k.Legalities.Legacy) || SideBoard.Any(k => !k.Legalities.Legacy))
                    {
                        Legal.Legacy = false;
                    }
                    else
                    {
                        Legal.Legacy = true;
                    }

                    if (Deck.Any(k => !k.Legalities.Historic) || SideBoard.Any(k => !k.Legalities.Historic))
                    {
                        Legal.Historic = false;
                    }
                    else
                    {
                        Legal.Historic = true;
                    }

                    if (Deck.Any(k => !k.Legalities.Modern) || SideBoard.Any(k => !k.Legalities.Modern))
                    {
                        Legal.Modern = false;
                    }
                    else
                    {
                        Legal.Modern = true;
                    }

                    if (Deck.Any(k => !k.Legalities.Pauper) || SideBoard.Any(k => !k.Legalities.Pauper))
                    {
                        Legal.Pauper = false;
                    }
                    else
                    {
                        Legal.Pauper = true;
                    }

                    if (Deck.Any(k => !k.Legalities.Pioneer) || SideBoard.Any(k => !k.Legalities.Pioneer))
                    {
                        Legal.Pioneer = false;
                    }
                    else
                    {
                        Legal.Pioneer = true;
                    }

                    if (Deck.Any(k => !k.Legalities.Standard) || SideBoard.Any(k => !k.Legalities.Standard))
                    {
                        Legal.Standard = false;
                    }
                    else
                    {
                        Legal.Standard = true;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckMaximunUnits()
        {
            try
            {
                string landkeyword = LocalizationManager.GetString("MtgCardType_Land").ToLower();

                foreach (CardDto MainCard in Deck)
                {
                    if (MainCard.TypeAndClass.SuperType.FullSuperType.ToLower().Contains(landkeyword))
                    {
                        continue;
                    }
                    else
                    {
                        if (MainCard.Units >= 4)
                        {
                            MainCard.Units = 4;
                            if (SideBoard != null && SideBoard.FirstOrDefault(k => k.Name == MainCard.Name) != null)
                            {
                                SideBoard.Remove(SideBoard.FirstOrDefault(k => k.Name == MainCard.Name));
                            }
                        }
                        else
                        {

                            if (SideBoard != null && SideBoard.FirstOrDefault(k => k.Name == MainCard.Name) != null)
                            {
                                int MaxUnits = 4 - MainCard.Units;
                                if (MaxUnits == 0)
                                {
                                    SideBoard.Remove(SideBoard.FirstOrDefault(k => k.Name == MainCard.Name));
                                }
                                else
                                {
                                    CardDto CardSideboard = SideBoard.FirstOrDefault(k => k.Name == MainCard.Name);
                                    if (CardSideboard.Units > MaxUnits)
                                    {
                                        CardSideboard.Units = MaxUnits;
                                    }
                                }

                            }

                        }
                    }
                }

                foreach (CardDto SideBoardCard in SideBoard)
                {
                    if (SideBoardCard.TypeAndClass.SuperType.FullSuperType.Contains(landkeyword))
                    {
                        continue;
                    }
                    else
                    {
                        if (SideBoardCard.Units >= 4)
                        {
                            SideBoardCard.Units = 4;
                        }
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
