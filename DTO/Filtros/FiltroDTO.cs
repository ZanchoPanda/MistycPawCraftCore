using MistycPawCraftCore.Utils.Enums;
using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO.Filtros
{
    public class FiltroDTO : INotifyPropertyChanged
    {
        #region Event
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
        #endregion

        #region Constructor
        public FiltroDTO()
        {

            Order = OrderTypesEnum.Name;
            TypeCard = MtgCardType.Other;

        }
        #endregion

        #region Properties

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private OrderTypesEnum _Order;
        public OrderTypesEnum Order
        {
            get
            {
                return _Order;
            }
            set
            {
                if (value != _Order)
                {
                    _Order = value;
                    OnPropertyChanged("Order");
                }
            }
        }

        private int _SelectedColors;
        public int SelectedColors
        {
            get
            {
                return _SelectedColors;
            }
            set
            {
                if (value != _SelectedColors)
                {
                    _SelectedColors = value;
                    OnPropertyChanged("SelectedColors");
                }
            }
        }

        #region Colors

        private bool _White;
        public bool White
        {
            get
            {
                return _White;
            }
            set
            {
                if (value != _White)
                {
                    _White = value;
                    OnPropertyChanged("White");
                    CheckColors();
                }
            }
        }

        private bool _Blue;
        public bool Blue
        {
            get
            {
                return _Blue;
            }
            set
            {
                if (value != _Blue)
                {
                    _Blue = value;
                    OnPropertyChanged("Blue");
                    CheckColors();
                }
            }
        }

        private bool _Green;
        public bool Green
        {
            get
            {
                return _Green;
            }
            set
            {
                if (value != _Green)
                {
                    _Green = value;
                    OnPropertyChanged("Green");
                    CheckColors();
                }
            }
        }

        private bool _Red;
        public bool Red
        {
            get
            {
                return _Red;
            }
            set
            {
                if (value != _Red)
                {
                    _Red = value;
                    OnPropertyChanged("Red");
                    CheckColors();
                }
            }
        }

        private bool _Black;
        public bool Black
        {
            get
            {
                return _Black;
            }
            set
            {
                if (value != _Black)
                {
                    _Black = value;
                    OnPropertyChanged("Black");
                    CheckColors();
                }
            }
        }

        #endregion

        private int? _ManaCost;
        public int? ManaCost
        {
            get
            {
                return _ManaCost;
            }
            set
            {
                if (value != _ManaCost)
                {
                    _ManaCost = value;
                    OnPropertyChanged("ManaCost");
                }
            }
        }

        private string _Power;
        public string Power
        {
            get
            {
                return _Power;
            }
            set
            {
                if (value != _Power)
                {
                    _Power = value;
                    OnPropertyChanged("Power");
                }
            }
        }

        private string _Thoughness;
        public string Thoughness
        {
            get
            {
                return _Thoughness;
            }
            set
            {
                if (value != _Thoughness)
                {
                    _Thoughness = value;
                    OnPropertyChanged("Thoughness");
                }
            }
        }

        private SetDTO _SetCard;
        public SetDTO SetCard
        {
            get
            {
                return _SetCard;
            }
            set
            {
                if (value != _SetCard)
                {
                    _SetCard = value;
                    OnPropertyChanged("SetCard");
                }
            }
        }

        private MtgCardType _TypeCard;
        public MtgCardType TypeCard
        {
            get
            {
                return _TypeCard;
            }
            set
            {
                if (value != _TypeCard)
                {
                    _TypeCard = value;
                    OnPropertyChanged("TypeCard");
                    TypeCardString = TypeCard.ToString();
                }
            }
        }

        private string _TypeCardString;
        public string TypeCardString
        {
            get
            {
                return _TypeCardString;
            }
            set
            {
                if (value != _TypeCardString)
                {
                    _TypeCardString = value;
                    OnPropertyChanged("TypeCardString");
                }
            }
        }

        private string _SuperTypeCardString;
        public string SuperTypeCardString
        {
            get
            {
                return _SuperTypeCardString;
            }
            set
            {
                if (value != _SuperTypeCardString)
                {
                    _SuperTypeCardString = value;
                    OnPropertyChanged("SuperTypeCardString");
                }
            }
        }


        private void CheckColors()
        {
            try
            {
                int CantTotal = 0;

                if (White)
                {
                    CantTotal++;
                }

                if (Black)
                {
                    CantTotal++;
                }

                if (Blue)
                {
                    CantTotal++;
                }

                if (Green)
                {
                    CantTotal++;
                }

                if (Red)
                {
                    CantTotal++;
                }

                SelectedColors = CantTotal;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
