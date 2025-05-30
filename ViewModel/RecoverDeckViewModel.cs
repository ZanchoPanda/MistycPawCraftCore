using MistycPawCraftCore.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MistycPawCraftCore.ViewModel
{
    public class RecoverDeckViewModel : INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Properties

        private ObservableCollection<DeckDTO> _RecoverDecks;
        public ObservableCollection<DeckDTO> RecoverDecks
        {
            get
            {
                return _RecoverDecks;
            }
            set
            {
                if (value != _RecoverDecks)
                {
                    _RecoverDecks = value;
                    OnPropertyChanged("RecoverDecks");
                }
            }
        }

        private DeckDTO _SelectedDeck;
        public DeckDTO SelectedDeck
        {
            get
            {
                return _SelectedDeck;
            }
            set
            {
                if (value != _SelectedDeck)
                {
                    _SelectedDeck = value;
                    OnPropertyChanged("SelectedDeck");
                }
            }
        }

        private List<DeckDTO> _SelectedDecks = new List<DeckDTO>();
        public List<DeckDTO> SelectedDecks
        {
            get { return _SelectedDecks; }
            set
            {
                _SelectedDecks = value;
                OnPropertyChanged(nameof(SelectedDecks));
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        private bool _Recover;
        public bool Recover
        {
            get
            {
                return _Recover;
            }
            set
            {
                if (value != _Recover)
                {
                    _Recover = value;
                    OnPropertyChanged("Recover");
                }
            }
        }

        #endregion

        #region Constructor

        public RecoverDeckViewModel()
        {
            RecoverDecks = new ObservableCollection<DeckDTO>();
            Recover = false;
        }

        public RecoverDeckViewModel(List<DeckDTO> DeletedDecks)
        {
            Recover = false;
            RecoverDecks = new ObservableCollection<DeckDTO>();
            DeletedDecks.ForEach(k => RecoverDecks.Add(k));

        }

        #endregion

        #region Commands


        internal void AddRegisterToRecoveryList()
        {
            try
            {
                if (SelectedDeck != null)
                {
                    if (SelectedDecks.Any(k => k == SelectedDeck))
                    {
                        SelectedDecks.Remove(SelectedDeck);
                    }

                    SelectedDecks.Add(SelectedDeck);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void RemoveRegisterToRecoveryList()
        {
            try
            {
                if (SelectedDeck != null)
                {
                    if (SelectedDecks.Any(k => k == SelectedDeck))
                    {
                        SelectedDecks.Remove(SelectedDeck);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

    }
}
