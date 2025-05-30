using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class SetDTO : INotifyPropertyChanged
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

        public SetDTO()
        {

        }

        private string _objectset;
        public string objectset
        {
            get
            {
                return _objectset;
            }
            set
            {
                if (value != _objectset)
                {
                    _objectset = value;
                    OnPropertyChanged("objectset");
                }
            }
        }

        private Guid _id;
        public Guid id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("id");
                }
            }
        }

        private string _CodeSet;
        public string CodeSet
        {
            get
            {
                return _CodeSet;
            }
            set
            {
                if (value != _CodeSet)
                {
                    _CodeSet = value;
                    OnPropertyChanged("CodeSet");
                }
            }
        }

        private string _NameSet;
        public string NameSet
        {
            get
            {
                return _NameSet;
            }
            set
            {
                if (value != _NameSet)
                {
                    _NameSet = value;
                    OnPropertyChanged("NameSet");
                }
            }
        }

        private Uri _UriSet;
        public Uri UriSet
        {
            get
            {
                return _UriSet;
            }
            set
            {
                if (value != _UriSet)
                {
                    _UriSet = value;
                    OnPropertyChanged("UriSet");
                }
            }
        }

        private Uri _ScryfallUri;
        public Uri ScryfallUri
        {
            get
            {
                return _ScryfallUri;
            }
            set
            {
                if (value != _ScryfallUri)
                {
                    _ScryfallUri = value;
                    OnPropertyChanged("ScryfallUri");
                }
            }
        }

        private Uri _SearchUri;
        public Uri SearchUri
        {
            get
            {
                return _SearchUri;
            }
            set
            {
                if (value != _SearchUri)
                {
                    _SearchUri = value;
                    OnPropertyChanged("SearchUri");
                }
            }
        }

        private DateTime _ReleasedDate;
        public DateTime ReleasedDate
        {
            get
            {
                return _ReleasedDate;
            }
            set
            {
                if (value != _ReleasedDate)
                {
                    _ReleasedDate = value;
                    OnPropertyChanged("ReleasedDate");
                }
            }
        }

        private string _SetType;
        public string SetType
        {
            get
            {
                return _SetType;
            }
            set
            {
                if (value != _SetType)
                {
                    _SetType = value;
                    OnPropertyChanged("SetType");
                }
            }
        }

        private int _TotalCards;
        public int TotalCards
        {
            get
            {
                return _TotalCards;
            }
            set
            {
                if (value != _TotalCards)
                {
                    _TotalCards = value;
                    OnPropertyChanged("TotalCards");
                }
            }
        }

        private bool _Digital;
        public bool Digital
        {
            get
            {
                return _Digital;
            }
            set
            {
                if (value != _Digital)
                {
                    _Digital = value;
                    OnPropertyChanged("Digital");
                }
            }
        }

        private bool _nonfoil_only;
        public bool nonfoil_only
        {
            get
            {
                return _nonfoil_only;
            }
            set
            {
                if (value != _nonfoil_only)
                {
                    _nonfoil_only = value;
                    OnPropertyChanged("nonfoil_only");
                }
            }
        }

        private bool _foil_only;
        public bool foil_only
        {
            get
            {
                return _foil_only;
            }
            set
            {
                if (value != _foil_only)
                {
                    _foil_only = value;
                    OnPropertyChanged("foil_only");
                }
            }
        }

        private Uri _svg_Uri;
        public Uri svg_Uri
        {
            get
            {
                return _svg_Uri;
            }
            set
            {
                if (value != _svg_Uri)
                {
                    _svg_Uri = value;
                    OnPropertyChanged("svg_Uri");
                }
            }
        }

        private string _ImageLocalPath;
        public string ImageLocalPath
        {
            get
            {
                return _ImageLocalPath;
            }
            set
            {
                if (value != _ImageLocalPath)
                {
                    _ImageLocalPath = value;
                    OnPropertyChanged("ImageLocalPath");
                }
            }
        }


    }

}
