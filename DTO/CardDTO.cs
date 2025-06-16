using MistycPawCraftCore.DTO.ResultApi;
using MistycPawCraftCore.Utils.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class CardDto : INotifyPropertyChanged
    {

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

        public CardDto()
        {
            Units = 1;
            CardMarketPrice = new PricesDTO();
            CardFaces = new List<CardFace>();
        }

        public Guid id { get; set; }
        public string Name { get; set; }
        public string PrintedName { get; set; }
        //public string ManaCost { get; set; }
        private string _ManaCost;
        public string ManaCost
        {
            get
            {
                return _ManaCost;
            }
            set
            {
                if (value != _ManaCost)
                {
                    _ManaCost = value;
                    OnPropertyChanged("ManaCost");
                }
            }
        }

        //public List<ManaViewDto> ManaCostView { get; set; }
        private List<ManaViewDto> _ManaCostView;
        public List<ManaViewDto> ManaCostView
        {
            get
            {
                return _ManaCostView;
            }
            set
            {
                if (value != _ManaCostView)
                {
                    _ManaCostView = value;
                    OnPropertyChanged("ManaCostView");
                }
            }
        }

        //public float CMC { get; set; }

        private float _CMC;
        public float CMC
        {
            get
            {
                return _CMC;
            }
            set
            {
                if (value != _CMC)
                {
                    _CMC = value;
                    OnPropertyChanged("CMC");
                }
            }
        }

        //public string OracleText { get; set; }
        private string _OracleText;
        public string OracleText
        {
            get
            {
                return _OracleText;
            }
            set
            {
                if (value != _OracleText)
                {
                    _OracleText = value;
                    OnPropertyChanged("OracleText");
                }
            }
        }

        //public string Power { get; set; }
        private string _Power;
        public string Power
        {
            get
            {
                return _Power;
            }
            set
            {
                if (value != _Power)
                {
                    _Power = value;
                    OnPropertyChanged("Power");
                }
            }
        }

        //public string Toughness { get; set; }
        private string _Toughness;
        public string Toughness
        {
            get
            {
                return _Toughness;
            }
            set
            {
                if (value != _Toughness)
                {
                    _Toughness = value;
                    OnPropertyChanged("Toughness");
                }
            }
        }

        //public SetDTO Set { get; set; }
        private SetDTO _Set;
        public SetDTO Set
        {
            get
            {
                return _Set;
            }
            set
            {
                if (value != _Set)
                {
                    _Set = value;
                    OnPropertyChanged("Set");
                }
            }
        }

        public string Rarity { get; set; }
        public string Artist { get; set; }
        //public string Flavor_Text { get; set; }

        private string _Flavor_Text;
        public string Flavor_Text
        {
            get
            {
                return _Flavor_Text;
            }
            set
            {
                if (value != _Flavor_Text)
                {
                    _Flavor_Text = value;
                    OnPropertyChanged("Flavor_Text");
                }
            }
        }

        private int _Units;
        public int Units
        {
            get
            {
                return _Units;
            }
            set
            {
                if (value != _Units)
                {
                    if (value <= 0)
                    {
                        value = 1;
                    }

                    if (TypeAndClass != null)
                    {
                        string landkeyword = LocalizationManager.GetString("MtgCardType_Land").ToLower();
                        string basickeyword = LocalizationManager.GetString("Basic").ToLower();
                        if (TypeAndClass.SuperType.FullSuperType.ToLower().Contains(landkeyword))
                        {
                            if (!TypeAndClass.SuperType.FullSuperType.ToLower().Contains(basickeyword))
                            {
                                if (value > 4)
                                {
                                    value = 4;
                                }
                            }
                        }
                        else
                        {
                            if (value > 4)
                            {
                                value = 4;
                            }
                        }
                    }

                    _Units = value;
                    OnPropertyChanged("Units");
                    OnPropertyChanged("TotalPriceCardMarket");
                }
            }
        }

        private decimal _TotalPriceCardMarket;
        public decimal TotalPriceCardMarket
        {
            get
            {
                return _TotalPriceCardMarket;
            }
            set
            {
                if (value != _TotalPriceCardMarket)
                {
                    _TotalPriceCardMarket = value;
                    OnPropertyChanged("TotalPriceCardMarket");

                    if (CardMarketPrice != null && CardMarketPrice.eur > 0)
                    {
                        TotalPriceCardMarket = CardMarketPrice.eur * Units;
                    }

                }
            }
        }


        public bool HasReprints { get; set; }
        public Uri RePrintsUri { get; set; }

        public PurchasesUrisDTO PurchasesUri { get; set; }

        public PricesDTO CardMarketPrice { get; set; }

        //public TypeAndSubTypeDto TypeAndClass { get; set; }
        private TypeAndSubTypeDto _TypeAndClass;
        public TypeAndSubTypeDto TypeAndClass
        {
            get
            {
                return _TypeAndClass;
            }
            set
            {
                if (value != _TypeAndClass)
                {
                    _TypeAndClass = value;
                    OnPropertyChanged("TypeAndClass");
                }
            }
        }

        public LegalitiesDto Legalities { get; set; }

        public ImageUriDto ImageUris { get; set; }

        public List<CardFace> CardFaces { get; set; }
    }

}
