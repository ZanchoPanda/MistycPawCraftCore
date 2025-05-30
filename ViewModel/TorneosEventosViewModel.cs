using MistycPawCraftCore.DDBB;
using MistycPawCraftCore.DTO;
using MistycPawCraftCore.Utils;
using MistycPawCraftCore.Utils.Enums;
using MistycPawCraftCore.Utils.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MistycPawCraftCore.ViewModel
{
    public class TorneosEventosViewModel : INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Constructor

        public TorneosEventosViewModel()
        {
            try
            {
                ListaEventos = new ObservableCollection<EventoDTO>();
                FechasConEventos = new List<DateTime>();

                CargarEventos();

                FechaSeleccionada = DateTime.Today;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Params

        private bool _HayFechaSeleccionada;
        public bool HayFechaSeleccionada
        {
            get
            {
                return _HayFechaSeleccionada;
            }
            set
            {
                if (value != _HayFechaSeleccionada)
                {
                    _HayFechaSeleccionada = value;
                    OnPropertyChanged("HayFechaSeleccionada");
                }
            }
        }

        private bool _HayEventoSeleccionado;
        public bool HayEventoSeleccionado
        {
            get
            {
                return _HayEventoSeleccionado;
            }
            set
            {
                if (value != _HayEventoSeleccionado)
                {
                    _HayEventoSeleccionado = value;
                    OnPropertyChanged("HayEventoSeleccionado");
                }
            }
        }


        private DateTime _FechaSeleccionada;
        public DateTime FechaSeleccionada
        {
            get
            {
                return _FechaSeleccionada;
            }
            set
            {
                if (value != _FechaSeleccionada)
                {
                    _FechaSeleccionada = value;
                    OnPropertyChanged("FechaSeleccionada");
                    if (FechaSeleccionada != null)
                    {
                        HayFechaSeleccionada = true;
                        FiltrarEventosPorFecha();
                    }
                }
            }
        }

        public ObservableCollection<EventoDTO> EventosFiltrados { get; set; } = new ObservableCollection<EventoDTO>();

        private ObservableCollection<EventoDTO> _ListaEventos;
        public ObservableCollection<EventoDTO> ListaEventos
        {
            get
            {
                return _ListaEventos;
            }
            set
            {
                if (value != _ListaEventos)
                {
                    _ListaEventos = value;
                    OnPropertyChanged("ListaEventos");
                }
            }
        }

        private EventoDTO _EventoSeleccionado;
        public EventoDTO EventoSeleccionado
        {
            get
            {
                return _EventoSeleccionado;
            }
            set
            {
                if (value != _EventoSeleccionado)
                {
                    _EventoSeleccionado = value;
                    OnPropertyChanged("EventoSeleccionado");

                    HayEventoSeleccionado = (EventoSeleccionado != null) ? true : false;

                    if (EventoSeleccionado != null)
                    {
                        HayFechaSeleccionada = true;
                    }
                }
            }
        }


        private List<DateTime> _FechasConEventos;
        public List<DateTime> FechasConEventos
        {
            get
            {
                return _FechasConEventos;
            }
            set
            {
                if (value != _FechasConEventos)
                {
                    _FechasConEventos = value;
                    OnPropertyChanged("FechasConEventos");
                }
            }
        }

        #endregion

        #region Funciones y más

        private void CargarEventos()
        {
            try
            {
                if (ListaEventos == null)
                {
                    ListaEventos = new ObservableCollection<EventoDTO>();
                }
                else
                {
                    ListaEventos.Clear();
                }

                if (FechasConEventos == null)
                {
                    FechasConEventos = new List<DateTime>();
                }
                else
                {
                    FechasConEventos.Clear();
                }

                foreach (EventoDTO Evento in EventoRepository.GetEventos())
                {

                    ListaEventos.Add(Evento);
                    if (!FechasConEventos.Contains(Evento.FechaHora.Date))
                    {
                        FechasConEventos.Add(Evento.FechaHora.Date);
                    }

                }
                //ListaEventos.ToList().AddRange(EventoRepository.GetEventos());
                FiltrarEventosPorFecha();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DateTime aux = FechaSeleccionada;
                FechaSeleccionada = DateTime.MinValue;
                FechaSeleccionada = aux;
            }
        }

        public void AgregarEvento(EventoDTO Evento)
        {
            try
            {
                if (Evento.Id == 0)
                {
                    EventoRepository.InsertEvento(Evento);
                }
                else
                {
                    EventoRepository.UpdateEvento(Evento);
                }

                CargarEventos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarEvento(int id)
        {
            try
            {
                EventoRepository.DeleteEvento(id);
                CargarEventos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FiltrarEventosPorFecha()
        {
            try
            {
                EventosFiltrados.Clear();
                foreach (var evento in ListaEventos.Where(e => e.FechaHora.Date == FechaSeleccionada.Date))
                {
                    EventosFiltrados.Add(evento);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Commands


        private ICommand _NuevoEventoCommand;
        public ICommand NuevoEventoCommand
        {
            get
            {
                _NuevoEventoCommand = new CommandHandler(() => NuevoEventoAction(), true);
                return _NuevoEventoCommand;
            }
        }

        private void NuevoEventoAction()
        {
            try
            {
                FechaSeleccionada = DateTime.Now;
                EventoSeleccionado = new EventoDTO()
                {
                    Nombre = string.Empty,
                    Fuente = string.Empty,
                    Link = string.Empty,
                    TipoEvento = EnumFormatoJuego.Standard,
                    Ubicacion = string.Empty,
                    FechaHora = FechaSeleccionada,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private ICommand _GuardarEventoCommand;
        public ICommand GuardarEventoCommand
        {
            get
            {
                _GuardarEventoCommand = new CommandHandler(() => GuardarEventoAction(), true);
                return _GuardarEventoCommand;
            }
        }

        private void GuardarEventoAction()
        {
            try
            {
                if (EventoSeleccionado == null)
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(EventoSeleccionado.Nombre))
                {
                    CustomDialog cd = new CustomDialog(EnumTitleMessage.Error, "Need Name's Event", EnumButtonsMessage.Acept, EnumButtonsMessage.Acept);
                    cd.ShowDialog();
                    return;
                }

                AgregarEvento(EventoSeleccionado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                EventoSeleccionado = null;
            }
        }


        private ICommand _BorrarEventoCommand;
        public ICommand BorrarEventoCommand
        {
            get
            {
                _BorrarEventoCommand = new CommandHandler(() => BorrarEventoAction(), true);
                return _BorrarEventoCommand;
            }
        }

        private void BorrarEventoAction()
        {
            try
            {
                if (EventoSeleccionado == null)
                {
                    return;
                }

                if (EventoSeleccionado.Id != 0)
                {
                    EliminarEvento(EventoSeleccionado.Id);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                EventoSeleccionado = null;
            }
        }



        #endregion

    }

}
