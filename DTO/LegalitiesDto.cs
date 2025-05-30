using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class LegalitiesDto : INotifyPropertyChanged
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

        public LegalitiesDto()
        {
            Standard = false;
            Historic = false;
            Pioneer = false;
            Modern = false;
            Legacy = false;
            Pauper = false;
            Commander = false;
        }


        private bool _Standard;
        public bool Standard
        {
            get
            {
                return _Standard;
            }
            set
            {
                if (value != _Standard)
                {
                    _Standard = value;
                    OnPropertyChanged("Standard");
                }
            }
        }

        private bool _Historic;
        public bool Historic
        {
            get
            {
                return _Historic;
            }
            set
            {
                if (value != _Historic)
                {
                    _Historic = value;
                    OnPropertyChanged("Historic");
                }
            }
        }

        private bool _Pioneer;
        public bool Pioneer
        {
            get
            {
                return _Pioneer;
            }
            set
            {
                if (value != _Pioneer)
                {
                    _Pioneer = value;
                    OnPropertyChanged("Pioneer");
                }
            }
        }

        private bool _Modern;

        public bool Modern
        {
            get
            {
                return _Modern;
            }
            set
            {
                if (value != _Modern)
                {
                    _Modern = value;
                    OnPropertyChanged("Modern");
                }
            }
        }

        private bool _Legacy;
        public bool Legacy
        {
            get
            {
                return _Legacy;
            }
            set
            {
                if (value != _Legacy)
                {
                    _Legacy = value;
                    OnPropertyChanged("Legacy");
                }
            }
        }

        private bool _Pauper;
        public bool Pauper
        {
            get
            {
                return _Pauper;
            }
            set
            {
                if (value != _Pauper)
                {
                    _Pauper = value;
                    OnPropertyChanged("Pauper");
                }
            }
        }

        private bool _Commander;
        public bool Commander
        {
            get
            {
                return _Commander;
            }
            set
            {
                if (value != _Commander)
                {
                    _Commander = value;
                    OnPropertyChanged("Commander");
                }
            }
        }


    }

}
