using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class SuperTypes : INotifyPropertyChanged
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

        public SuperTypes()
        {

        }

        private string _FullSuperType;
        public string FullSuperType
        {
            get
            {
                return _FullSuperType;
            }
            set
            {
                if (value != _FullSuperType)
                {
                    _FullSuperType = value;
                    OnPropertyChanged("FullSuperType");
                    if (!string.IsNullOrWhiteSpace(FullSuperType))
                    {
                        string[] Split = FullSuperType.Trim().Split(' ');
                        foreach (string SplittedPart in Split)
                        {
                            if (string.IsNullOrWhiteSpace(SuperType1))
                            {
                                SuperType1 = CapitalizeString(SplittedPart);
                            }
                            else if (string.IsNullOrWhiteSpace(SuperType2))
                            {
                                SuperType2 = CapitalizeString(SplittedPart);
                            }
                            else
                            {
                                SuperType3 = CapitalizeString(SplittedPart);
                            }
                        }
                    }

                }
            }
        }

        private string _SuperType1;
        public string SuperType1
        {
            get
            {
                return _SuperType1;
            }
            set
            {
                if (value != _SuperType1)
                {
                    _SuperType1 = value;
                    OnPropertyChanged("SuperType1");
                }
            }
        }

        private string _SuperType2;
        public string SuperType2
        {
            get
            {
                return _SuperType2;
            }
            set
            {
                if (value != _SuperType2)
                {
                    _SuperType2 = value;
                    OnPropertyChanged("SuperType2");
                }
            }
        }

        private string _SuperType3;
        public string SuperType3
        {
            get
            {
                return _SuperType3;
            }
            set
            {
                if (value != _SuperType3)
                {
                    _SuperType3 = value;
                    OnPropertyChanged("SuperType3");
                }
            }
        }

        private string CapitalizeString(string Texto)
        {
            try
            {
                return $"{char.ToUpper(Texto[0])}{Texto.Substring(1)}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
