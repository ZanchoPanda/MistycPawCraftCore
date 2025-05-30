using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class TypeAndSubTypeDto : INotifyPropertyChanged
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

        public TypeAndSubTypeDto()
        {
            IsLegendary = false;
            SuperType = new SuperTypes();
            CardType = new CardTypes();
        }

        public bool IsLegendary { get; set; }

        private SuperTypes _SuperType;
        public SuperTypes SuperType
        {
            get
            {
                return _SuperType;
            }
            set
            {
                if (value != _SuperType)
                {
                    _SuperType = value;
                    OnPropertyChanged("SuperType");

                    if (SuperType != null)
                    {
                        IsLegendary = (SuperType.FullSuperType != null && SuperType.FullSuperType.Contains("legendary")) ? true : false;
                    }
                }
            }
        }

        private CardTypes _CardType;
        public CardTypes CardType
        {
            get
            {
                return _CardType;
            }
            set
            {
                if (value != _CardType)
                {
                    _CardType = value;
                    OnPropertyChanged("CardType");
                }
            }
        }

    }

}
