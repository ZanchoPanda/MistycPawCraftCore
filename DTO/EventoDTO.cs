using MistycPawCraftCore.Utils.Enums;
using System;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO
{
    public class EventoDTO : INotifyPropertyChanged
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

        public EventoDTO()
        {

        }

        #endregion

        #region Params

        private int _Id;
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (value != _Id)
                {
                    _Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private string _Nombre;
        public string Nombre
        {
            get
            {
                return _Nombre;
            }
            set
            {
                if (value != _Nombre)
                {
                    _Nombre = value;
                    OnPropertyChanged("Nombre");
                }
            }
        }

        private DateTime _FechaHora;
        public DateTime FechaHora
        {
            get
            {
                return _FechaHora;
            }
            set
            {
                if (value != _FechaHora)
                {
                    _FechaHora = value;
                    OnPropertyChanged("FechaHora");
                }
            }
        }

        private string _Ubicacion;
        public string Ubicacion
        {
            get
            {
                return _Ubicacion;
            }
            set
            {
                if (value != _Ubicacion)
                {
                    _Ubicacion = value;
                    OnPropertyChanged("Ubicacion");
                }
            }
        }

        private EnumFormatoJuego _TipoEvento;
        public EnumFormatoJuego TipoEvento
        {
            get
            {
                return _TipoEvento;
            }
            set
            {
                if (value != _TipoEvento)
                {
                    _TipoEvento = value;
                    OnPropertyChanged("TipoEvento");
                }
            }
        }

        private string _Fuente;
        public string Fuente
        {
            get
            {
                return _Fuente;
            }
            set
            {
                if (value != _Fuente)
                {
                    _Fuente = value;
                    OnPropertyChanged("Fuente");
                }
            }
        }

        private string _Link;
        public string Link
        {
            get
            {
                return _Link;
            }
            set
            {
                if (value != _Link)
                {
                    _Link = value;
                    OnPropertyChanged("Link");
                }
            }
        }

        private bool _RecordatorioActivado;
        public bool RecordatorioActivado
        {
            get
            {
                return _RecordatorioActivado;
            }
            set
            {
                if (value != _RecordatorioActivado)
                {
                    _RecordatorioActivado = value;
                    OnPropertyChanged("RecordatorioActivado");
                }
            }
        }


        #endregion

    }
}
