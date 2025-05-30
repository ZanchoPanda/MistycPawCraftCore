using MistycPawCraftCore.Utils.Language;
using System;
using System.ComponentModel;
using System.Threading;

namespace MistycPawCraftCore.ViewModel
{
    public class NoticiasViewModel : INotifyPropertyChanged
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

        private string _webNewsUrl;
        public string WebNewsUrl
        {
            get => _webNewsUrl;
            set
            {
                _webNewsUrl = value;
                OnPropertyChanged(nameof(WebNewsUrl));
            }
        }

        public void UpdateNewsUrl()
        {
            string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            WebNewsUrl = $"https://magic.wizards.com/{lang}/news";
        }

        public NoticiasViewModel()
        {
            LocalizationManager.LanguageChanged += (_, __) => UpdateNewsUrl();
            UpdateNewsUrl();
        }

    }
}
