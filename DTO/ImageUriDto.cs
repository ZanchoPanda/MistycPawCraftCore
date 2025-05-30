using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class ImageUriDto : INotifyPropertyChanged
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

        public ImageUriDto()
        {

        }

        private Uri _Small;
        public Uri Small
        {
            get
            {
                return _Small;
            }
            set
            {
                if (value != _Small)
                {
                    _Small = value;
                    OnPropertyChanged("Small");
                }
            }
        }

        private Uri _Normal;
        public Uri Normal
        {
            get
            {
                return _Normal;
            }
            set
            {
                if (value != _Normal)
                {
                    _Normal = value;
                    OnPropertyChanged("Normal");
                }
            }
        }

        private Uri _Large;
        public Uri Large
        {
            get
            {
                return _Large;
            }
            set
            {
                if (value != _Large)
                {
                    _Large = value;
                    OnPropertyChanged("Large");
                }
            }
        }

        private Uri _PNG;
        public Uri PNG
        {
            get
            {
                return _PNG;
            }
            set
            {
                if (value != _PNG)
                {
                    _PNG = value;
                    OnPropertyChanged("PNG");
                }
            }
        }

    }

}
