using MaterialDesignThemes.Wpf;
using MistycPawCraftCore.Utils;
using MistycPawCraftCore.Utils.Enums;
using MistycPawCraftCore.Utils.Language;
using MistycPawCraftCore.Utils.Message;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MistycPawCraftCore.ViewModel
{
    public class ConfigurationOthersViewModel : INotifyPropertyChanged
    {

        #region Evento Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        #region Params

        //private string _PaypalDonate;
        //public string PaypalDonate
        //{
        //    get
        //    {
        //        return _PaypalDonate;
        //    }
        //    set
        //    {
        //        if (value != _PaypalDonate)
        //        {
        //            _PaypalDonate = value;
        //            OnPropertyChanged("PaypalDonate");
        //        }
        //    }
        //}

        private PaletteHelper _paletteHelper = new PaletteHelper();

        #region Theme

        private bool _DarkTheme;
        public bool DarkTheme
        {
            get
            {
                return _DarkTheme;
            }
            set
            {
                if (value != _DarkTheme)
                {
                    _DarkTheme = value;
                    UpdateTheme();
                    OnPropertyChanged("DarkTheme");
                }
            }
        }

        #region Colors

        //private bool _CustomColor;
        //public bool CustomColor
        //{
        //    get
        //    {
        //        return _CustomColor;
        //    }
        //    set
        //    {
        //        if (value != _CustomColor)
        //        {
        //            _CustomColor = value;
        //            UpdateTheme();
        //            OnPropertyChanged("CustomColor");
        //        }
        //    }
        //}

        //private Color _SelectedColor;
        //public Color SelectedColor
        //{
        //    get
        //    {
        //        return _SelectedColor;
        //    }
        //    set
        //    {
        //        if (value != _SelectedColor)
        //        {
        //            _SelectedColor = value;
        //            UpdateTheme();
        //            OnPropertyChanged("SelectedColor");
        //        }
        //    }
        //}

        #endregion

        #endregion

        #region Language

        private string _SelectedLanguage;
        public string SelectedLanguage
        {
            get
            {
                return _SelectedLanguage;
            }
            set
            {
                if (value != _SelectedLanguage)
                {
                    _SelectedLanguage = value;
                    UpdateLanguage();
                    OnPropertyChanged("SelectedLanguage");
                }
            }
        }

        private ObservableCollection<string> _Languages;
        public ObservableCollection<string> Languages
        {
            get
            {
                return _Languages;
            }
            set
            {
                if (value != _Languages)
                {
                    _Languages = value;
                    OnPropertyChanged("Languages");
                }
            }
        }

        #endregion

        #endregion

        #region Constructor
        public ConfigurationOthersViewModel()
        {
            try
            {
                Languages = new ObservableCollection<string> { "en", "es" };

                //PaypalDonate = Properties.Settings.Default.PaypalDonate;
                SelectedLanguage = Properties.Settings.Default.Language ?? string.Empty;
                DarkTheme = Properties.Settings.Default.DarkMode;

                //CustomColor = Properties.Settings.Default.IsCustomColor;
                //SelectedColor = (CustomColor) ?
                //                    (Color)ColorConverter.ConvertFromString(Properties.Settings.Default.CustomColor) :
                //                    (Color)ColorConverter.ConvertFromString("#36013F");

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Commands


        private ICommand _DonateCommand;
        public ICommand DonateCommand
        {
            get
            {
                _DonateCommand = new CommandHandler(() => DonateAction(), true);
                return _DonateCommand;
            }
        }

        private void DonateAction()
        {
            try
            {
                //BrowserHelper.LaunchUrl(BrowserHelper.GetDefaultBrowserPath(), PaypalDonate);
                BrowserHelper.LaunchUrl(BrowserHelper.GetDefaultBrowserPath(), Ppm.PaypalDonateUrl);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        private ICommand _SaveThemeCommand;
        public ICommand SaveThemeCommand
        {
            get
            {
                _SaveThemeCommand = new CommandHandler(() => SaveThemeAction(), true);
                return _SaveThemeCommand;
            }
        }

        private void SaveThemeAction()
        {
            try
            {
                UpdateTheme();

                Properties.Settings.Default.Save();

            }
            catch (Exception ex)
            {

            }
        }


        private ICommand _RecargarSetsCommand;
        public ICommand RecargarSetsCommand
        {
            get
            {
                _RecargarSetsCommand = new CommandHandler(async () => await RecargarSetsAction(), true);
                return _RecargarSetsCommand;
            }
        }

        private async Task RecargarSetsAction()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await new ScryfallApiClient().LoadSetsAsync(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = null;

                string mensaje = LocalizationManager.GetString("ProcessOK");
                CustomDialog cstm = new CustomDialog(EnumTitleMessage.Finish_Process, mensaje, Utils.Enums.EnumButtonsMessage.Acept, Utils.Enums.EnumButtonsMessage.Acept);
                cstm.ShowDialog();
            }
        }

        #endregion

        #region Actions



        #endregion

        #region Methods

        private void UpdateTheme()
        {
            try
            {
                Theme Newtheme = new Theme();

                #region Intento de colores

                //if (DarkTheme)
                //{
                //    Newtheme.SetBaseTheme(new MaterialDesignDarkTheme());
                //}
                //else
                //{
                //    Newtheme.SetBaseTheme(new MaterialDesignLightTheme());
                //}

                //if (CustomColor)
                //{
                //    Newtheme.SetPrimaryColor(SelectedColor);
                //}

                //_paletteHelper.SetTheme(Newtheme);

                //Properties.Settings.Default.DarkMode = DarkTheme;
                //Properties.Settings.Default.IsCustomColor = CustomColor;
                //Properties.Settings.Default.CustomColor = CustomColor ? SelectedColor.ToString() : string.Empty;
                #endregion

                #region Solo Dark/Light)

                ITheme theme = _paletteHelper.GetTheme();
                theme.SetBaseTheme(DarkTheme ? Theme.Dark : Theme.Light);

                _paletteHelper.SetTheme(theme);

                Properties.Settings.Default.DarkMode = DarkTheme;
                Properties.Settings.Default.Save();

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateLanguage()
        {
            // Set the culture based on the selected language
            CultureInfo newCulture = new CultureInfo(SelectedLanguage);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            Properties.Settings.Default.Language = SelectedLanguage;
            Properties.Settings.Default.Save();

            LocalizationManager.ChangeLanguage(SelectedLanguage);
        }

        #endregion

        #region Events

        #endregion
    }
}
