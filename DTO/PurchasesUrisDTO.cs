using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class PurchasesUrisDTO : INotifyPropertyChanged
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

        #region Constructor

        public PurchasesUrisDTO()
        {

        }

        #endregion

        #region Propiedades

        private Uri _cardhoader;
        public Uri cardhoader
        {
            get
            {
                return _cardhoader;
            }
            set
            {
                if (value != _cardhoader)
                {
                    _cardhoader = value;
                    OnPropertyChanged("cardhoader");
                }
            }
        }

        private Uri _cardmarket;
        public Uri cardmarket
        {
            get
            {
                return _cardmarket;
            }
            set
            {
                if (value != _cardmarket)
                {
                    _cardmarket = value;
                    OnPropertyChanged("cardmarket");
                }
            }
        }

        private Uri _tcgplayer;
        public Uri tcgplayer
        {
            get
            {
                return _tcgplayer;
            }
            set
            {
                if (value != _tcgplayer)
                {
                    _tcgplayer = value;
                    OnPropertyChanged("tcgplayer");
                }
            }
        }

        #endregion

    }
}
