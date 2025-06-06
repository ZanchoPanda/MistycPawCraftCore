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
        public string ManaCost { get; set; }
        public List<ManaViewDto> ManaCostView { get; set; }
        public float CMC { get; set; }
        public string OracleText { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public SetDTO Set { get; set; }

        public string Rarity { get; set; }
        public string Artist { get; set; }
        public string Flavor_Text { get; set; }

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

        public TypeAndSubTypeDto TypeAndClass { get; set; }

        public LegalitiesDto Legalities { get; set; }

        public ImageUriDto ImageUris { get; set; }

        public List<CardFace> CardFaces { get; set; }
    }

}
