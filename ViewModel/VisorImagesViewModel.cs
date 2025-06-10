using MistycPawCraftCore.DTO;
using MistycPawCraftCore.Properties;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace MistycPawCraftCore.ViewModel
{
    public class VisorImagesViewModel : INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Parametros

        private CardDto _Card;
        public CardDto Card
        {
            get
            {
                return _Card;
            }
            set
            {
                if (value != _Card)
                {
                    _Card = value;
                    OnPropertyChanged("Card");

                    if (Card != null && Card.CMC > 0)
                    {
                        string SymbolPath = Settings.Default.ImageSymbolPath;

                        MatchCollection matches = Regex.Matches(Card.ManaCost, @"\{(.*?)\}");
                        foreach (Match match in matches)
                        {
                            string symbol = match.Groups[1].Value.ToUpper(); // Asegura mayúsculas
                            string iconPath = $"{SymbolPath}{symbol}.png";
                            ManaIcons.Add(iconPath);
                        }
                    }

                }
            }
        }

        private ObservableCollection<string> _ManaIcons;
        public ObservableCollection<string> ManaIcons
        {
            get
            {
                return _ManaIcons;
            }
            set
            {
                if (value != _ManaIcons)
                {
                    _ManaIcons = value;
                    OnPropertyChanged("ManaIcons");
                }
            }
        }

        #endregion

        #region Constructor

        public VisorImagesViewModel()
        {

        }

        public VisorImagesViewModel(CardDto CardDTO)
        {
            ManaIcons = new ObservableCollection<string>();

            Card = CardDTO;
        }

        #endregion


    }
}
