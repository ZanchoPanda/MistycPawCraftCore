using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MistycPawCraftCore.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
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

        ObservableCollection<object> _Pestana;
        public ObservableCollection<object> Pestana
        {
            get
            {
                return _Pestana;
            }
            set
            {
                _Pestana = value;
                OnPropertyChanged("Pestana");
            }

        }

        public MainViewModel()
        {
            Pestana = new ObservableCollection<object>()
            {
                new NoticiasViewModel(),
                new PrincipalViewModel(),
                new DeckViewModel(),
                new StadisticViewModel(),
                new TorneosEventosViewModel(),
                new ConfigurationOthersViewModel(),
            };
        }

    }
}
