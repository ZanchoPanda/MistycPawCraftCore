using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class ReprintsDTO : INotifyPropertyChanged
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

        public ReprintsDTO()
        {

        }

        private string _Artist;
        public string Artist
        {
            get
            {
                return _Artist;
            }
            set
            {
                if (value != _Artist)
                {
                    _Artist = value;
                    OnPropertyChanged("Artist");
                }
            }
        }

        private DateTime _ReleaseAt;
        public DateTime ReleaseAt
        {
            get
            {
                return _ReleaseAt;
            }
            set
            {
                if (value != _ReleaseAt)
                {
                    _ReleaseAt = value;
                    OnPropertyChanged("ReleaseAt");
                }
            }
        }

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

        private ImageUriDto _ImageUris;
        public ImageUriDto ImageUris
        {
            get
            {
                return _ImageUris;
            }
            set
            {
                if (value != _ImageUris)
                {
                    _ImageUris = value;
                    OnPropertyChanged("ImageUris");
                }
            }
        }

        private PurchasesUrisDTO _PurchaseUris;
        public PurchasesUrisDTO PurchaseUris
        {
            get
            {
                return _PurchaseUris;
            }
            set
            {
                if (value != _PurchaseUris)
                {
                    _PurchaseUris = value;
                    OnPropertyChanged("PurchaseUris");
                }
            }
        }

        private string _LocalImagePath;
        public string LocalImagePath
        {
            get
            {
                return _LocalImagePath;
            }
            set
            {
                if (value != _LocalImagePath)
                {
                    _LocalImagePath = value;
                    OnPropertyChanged("LocalImagePath");
                }
            }
        }

    }
}
