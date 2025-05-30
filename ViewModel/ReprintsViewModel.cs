using MistycPawCraftCore.DTO;
using MistycPawCraftCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MistycPawCraftCore.ViewModel
{
    public class ReprintsViewModel : INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Properties

        private ObservableCollection<ReprintsDTO> _ReprintsList;
        public ObservableCollection<ReprintsDTO> ReprintsList
        {
            get
            {
                return _ReprintsList;
            }
            set
            {
                if (value != _ReprintsList)
                {
                    _ReprintsList = value;
                    OnPropertyChanged("ReprintsList");
                }
            }
        }

        private ReprintsDTO _ReprintSelected;
        public ReprintsDTO ReprintSelected
        {
            get
            {
                return _ReprintSelected;
            }
            set
            {
                if (value != _ReprintSelected)
                {
                    _ReprintSelected = value;
                    OnPropertyChanged("ReprintSelected");

                }
            }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get
            {
                return _IsLoading;
            }
            set
            {
                if (value != _IsLoading)
                {
                    _IsLoading = value;
                    OnPropertyChanged("IsLoading");
                }
            }
        }

        #region Paginacion

        private List<ReprintsDTO> _allReprints = new List<ReprintsDTO>();
        private int _loadedCount = 0;
        private const int PageSize = 10;
        private readonly Uri _reprintsUri;
        private readonly ScryfallApiClient _apiClient;

        public ICommand LoadMoreCommand { get; }
        //private bool isLoading = false;
        private bool hasMore = true;

        #endregion

        #endregion

        #region Constructor

        public ReprintsViewModel()
        {
            ReprintsList = new ObservableCollection<ReprintsDTO>();
        }

        public ReprintsViewModel(List<ReprintsDTO> Lista) : this()
        {
            if (ReprintsList != null)
            {
                ReprintsList.Clear();
                Lista.ForEach(k => ReprintsList.Add(k));
            }
            else
            {
                ReprintsList = new ObservableCollection<ReprintsDTO>(Lista);
            }
        }

        public ReprintsViewModel(Uri reprintsUri, ScryfallApiClient apiClient)
        {
            try
            {
                IsLoading = true;

                ReprintsList = new ObservableCollection<ReprintsDTO>();
                _reprintsUri = reprintsUri;
                _apiClient = apiClient;
                //LoadMoreCommand = new AsyncRelayCommand(LoadReprintsAsync(reprintsUri));
                //_ = LoadReprintsAsync();
                _ = InicializarAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool HasMore => hasMore;

        #endregion

        #region Command
        //private async Task LoadMoreAsync()
        //{
        //    try
        //    {
        //        if (IsLoading || !hasMore) return;
        //        IsLoading = true;

        //        var result = await _apiClient.FillReprintsPaginatedAsync(_reprintsUri, PageSize, currentPage);
        //        foreach (var card in result)
        //            ReprintsList.Add(card);

        //        currentPage++;
        //        hasMore = result.Count >= PageSize;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        IsLoading = false;
        //    }
        //}


        private async Task InicializarAsync()
        {
            IsLoading = true;
            _allReprints = await _apiClient.FillReprintsAsync(_reprintsUri.ToString());
            _loadedCount = 0;
            ReprintsList.Clear();

            await LoadMoreAsync();
            IsLoading = false;
        }
        public async Task LoadMoreAsync()
        {
            try
            {
                if (_loadedCount >= _allReprints.Count)
                    return; // no hay más items

                IsLoading = true;

                int toLoad = Math.Min(PageSize, _allReprints.Count - _loadedCount);
                var nextBatch = _allReprints.GetRange(_loadedCount, toLoad);

                foreach (var item in nextBatch)
                {
                    Application.Current.Dispatcher.Invoke(() => ReprintsList.Add(item));
                    await Task.Delay(30); // suavizar
                }

                _loadedCount += toLoad;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task LoadReprintsAsync()
        {
            try
            {

                IsLoading = true;

                var allReprints = await _apiClient.FillReprintsAsync(_reprintsUri.ToString());

                // Actualizar colección en UI thread
                Application.Current.Dispatcher.Invoke(() => ReprintsList.Clear());

                foreach (var reprint in allReprints)
                {
                    // Añadir en UI thread para evitar problemas con ObservableCollection
                    Application.Current.Dispatcher.Invoke(() => ReprintsList.Add(reprint));

                    await Task.Delay(30); // Pequeña pausa para suavizar carga progresiva
                }

                IsLoading = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

    }

    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public async void Execute(object parameter) => await _execute();
    }

}
