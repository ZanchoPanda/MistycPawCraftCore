using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class CardTypes : INotifyPropertyChanged
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

        public CardTypes()
        {

        }

        private string _FullCardType;
        public string FullCardType
        {
            get
            {
                return _FullCardType;
            }
            set
            {
                if (value != _FullCardType)
                {
                    _FullCardType = value;
                    OnPropertyChanged("FullCardType");
                    if (!string.IsNullOrWhiteSpace(FullCardType))
                    {
                        string[] Split = FullCardType.Trim().Split(' ');
                        foreach (string SplittedPart in Split)
                        {
                            if (string.IsNullOrWhiteSpace(CardType1))
                            {
                                CardType1 = CapitalizeString(SplittedPart);
                            }
                            else if (string.IsNullOrWhiteSpace(CardType2))
                            {
                                CardType2 = CapitalizeString(SplittedPart);
                            }
                            else
                            {
                                CardType3 = CapitalizeString(SplittedPart);
                            }
                        }
                    }
                }
            }
        }

        private string _CardType1;
        public string CardType1
        {
            get
            {
                return _CardType1;
            }
            set
            {
                if (value != _CardType1)
                {
                    _CardType1 = value;
                    OnPropertyChanged("CardType1");
                }
            }
        }

        private string _CardType2;
        public string CardType2
        {
            get
            {
                return _CardType2;
            }
            set
            {
                if (value != _CardType2)
                {
                    _CardType2 = value;
                    OnPropertyChanged("CardType2");
                }
            }
        }

        private string _CardType3;
        public string CardType3
        {
            get
            {
                return _CardType3;
            }
            set
            {
                if (value != _CardType3)
                {
                    _CardType3 = value;
                    OnPropertyChanged("CardType3");
                }
            }
        }

        private string CapitalizeString(string Texto)
        {
            try
            {
                return $"{char.ToUpper(Texto[0])}{Texto.Substring(1)}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
