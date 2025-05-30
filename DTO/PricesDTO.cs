using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class PricesDTO : INotifyPropertyChanged
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

        public PricesDTO()
        {
            eur = 0.0M;
            eurFoil = 0.0M;
            usd = 0.0M;
            usdFoil = 0.0M;
        }

        #endregion

        #region Properties

        private decimal _eur;
        public decimal eur
        {
            get
            {
                return _eur;
            }
            set
            {
                if (value != _eur)
                {
                    _eur = value;
                    OnPropertyChanged("eur");
                }
            }
        }

        private decimal _eurFoil;
        public decimal eurFoil
        {
            get
            {
                return _eurFoil;
            }
            set
            {
                if (value != _eurFoil)
                {
                    _eurFoil = value;
                    OnPropertyChanged("eurFoil");
                }
            }
        }

        private decimal _usd;
        public decimal usd
        {
            get
            {
                return _usd;
            }
            set
            {
                if (value != _usd)
                {
                    _usd = value;
                    OnPropertyChanged("usd");
                }
            }
        }

        private decimal _usdFoil;
        public decimal usdFoil
        {
            get
            {
                return _usdFoil;
            }
            set
            {
                if (value != _usdFoil)
                {
                    _usdFoil = value;
                    OnPropertyChanged("usdFoil");
                }
            }
        }

        #endregion

    }
}
