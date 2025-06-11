using MistycPawCraftCore.DTO;
using MistycPawCraftCore.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private bool _VerImagen;
        public bool VerImagen
        {
            get
            {
                return _VerImagen;
            }
            set
            {
                if (value != _VerImagen)
                {
                    _VerImagen = value;
                    OnPropertyChanged("VerImagen");
                }
            }
        }

        private int _currentFaceIndex;
        public int CurrentFaceIndex
        {
            get => _currentFaceIndex;
            set
            {
                if (_currentFaceIndex != value)
                {
                    _currentFaceIndex = value;
                    OnPropertyChanged(nameof(CurrentImageUri));
                }
            }
        }
        public ImageSource CurrentImageUri
        {
            get
            {
                if (Card == null)
                {
                    return null;
                }

                if (Card?.CardFaces != null && Card.CardFaces.Count > 0)
                {
                    var path = Card.CardFaces[CurrentFaceIndex].image_uris?.normal;
                    return string.IsNullOrWhiteSpace(path) ? null : new BitmapImage(new Uri(path));
                }

                return string.IsNullOrWhiteSpace(Card?.ImageUris.Normal.ToString())
                    ? null
                    : new BitmapImage(new Uri(Card.ImageUris.Normal.ToString()));
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

        public VisorImagesViewModel(CardDto CardDTO, bool VerImagen = true)
        {
            ManaIcons = new ObservableCollection<string>();

            Card = CardDTO;
        }

        #endregion

        #region Eventos

        public void ToggleFace()
        {
            if (Card?.CardFaces?.Count > 1)
            {
                CurrentFaceIndex = (CurrentFaceIndex + 1) % Card.CardFaces.Count;
            }
        }

        #endregion

    }
}
