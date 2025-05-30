using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class ManaViewDto : INotifyPropertyChanged
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

        private string _Symbol;
        public string Symbol
        {
            get
            {
                return _Symbol;
            }
            set
            {
                if (value != _Symbol)
                {
                    _Symbol = value;
                    OnPropertyChanged("Symbol");
                }
            }
        }

        private string _SymbolDefinition;
        public string SymbolDefinition
        {
            get
            {
                return _SymbolDefinition;
            }
            set
            {
                if (value != _SymbolDefinition)
                {
                    _SymbolDefinition = value;
                    OnPropertyChanged("SymbolDefinition");
                }
            }
        }

        private string _SymbolPath;
        public string SymbolPath
        {
            get
            {
                return _SymbolPath;
            }
            set
            {
                if (value != _SymbolPath)
                {
                    _SymbolPath = value;
                    OnPropertyChanged("SymbolPath");
                }
            }
        }

    }
}
