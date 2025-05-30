using MaterialDesignThemes.Wpf;
using MistycPawCraftCore.Utils.Enums;
using MistycPawCraftCore.Utils.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MistycPawCraftCore.Utils.Message
{
    /// <summary>
    /// Lógica de interacción para CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #region Properties

        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (value != _Title)
                {
                    _Title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        private string _Message;
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                if (value != _Message)
                {
                    _Message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private PackIconKind _Image;
        public PackIconKind Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value != _Image)
                {
                    _Image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        public EnumMessageResult MessageResult { get; set; }

        #endregion

        #region Constructor

        public CustomDialog(string TitleRequest, string MessageRequest, EnumButtonsMessage ButtonsResult)
        {
            ConstructorComun(null, TitleRequest, MessageRequest, ButtonsResult);
        }

        public CustomDialog(EnumTitleMessage EnumTitle, string MessageRequest, EnumButtonsMessage ButtonsResult, EnumButtonsMessage ButtonDefault = EnumButtonsMessage.Acept)
        {
            ConstructorComun(EnumTitle, null, MessageRequest, ButtonsResult, ButtonDefault);
        }

        public CustomDialog(EnumTitleMessage EnumTitle, string TitleRequest, string MessageRequest, EnumButtonsMessage ButtonsResult)
        {
            ConstructorComun(EnumTitle, TitleRequest, MessageRequest, ButtonsResult);
        }

        #endregion

        #region Events

        private void ConstructorComun(EnumTitleMessage? EnumTitle, string TitleRequest, string MessageRequest, EnumButtonsMessage ButtonsResult, EnumButtonsMessage ButtonDefault = EnumButtonsMessage.Acept)
        {
            try
            {
                InitializeComponent();
                MessageResult = EnumMessageResult.Cancel;

                DataContext = this;

                if (!EnumTitle.HasValue)
                {
                    EnumTitle = EnumTitleMessage.Indeterminated;
                }

                PackIcon pkIcon = new PackIcon();
                pkIcon.Kind = PackIconKind.Cross;

                switch (EnumTitle)
                {
                    case EnumTitleMessage.Indeterminated:
                        pkIcon.Kind = PackIconKind.Home;
                        break;
                    case EnumTitleMessage.Error:
                        pkIcon.Kind = PackIconKind.Error;
                        break;
                    case EnumTitleMessage.Warning:
                        pkIcon.Kind = PackIconKind.Warning;
                        break;
                    case EnumTitleMessage.Finish_Process:
                        pkIcon.Kind = PackIconKind.Tick;
                        break;
                    case EnumTitleMessage.Ask:
                        pkIcon.Kind = PackIconKind.FrequentlyAskedQuestions;
                        break;
                    case EnumTitleMessage.Info:
                        pkIcon.Kind = PackIconKind.Information;
                        break;
                    default:
                        pkIcon.Kind = PackIconKind.Information;
                        break;
                }

                Image = pkIcon.Kind;

                if (string.IsNullOrWhiteSpace(TitleRequest))
                {
                    switch (EnumTitle.Value)
                    {
                        case EnumTitleMessage.Indeterminated:
                            Title = LocalizationManager.GetString("Message");
                            break;
                        case EnumTitleMessage.Error:
                            Title = LocalizationManager.GetString("Error");
                            break;
                        case EnumTitleMessage.Warning:
                            Title = LocalizationManager.GetString("Warning");
                            break;
                        case EnumTitleMessage.Finish_Process:
                            Title = LocalizationManager.GetString("ProcessEnded");
                            break;
                        case EnumTitleMessage.Info:
                        default:
                            Title = LocalizationManager.GetString("Info");
                            break;
                    }
                }
                else
                {
                    Title = TitleRequest;
                }

                switch (ButtonsResult)
                {
                    case EnumButtonsMessage.YesNo:
                        Aceptar.Visibility = Visibility.Collapsed;
                        Cancelar.Visibility = Visibility.Collapsed;
                        Si.Focus();
                        break;
                    case EnumButtonsMessage.YesNoCancel:
                        Aceptar.Visibility = Visibility.Collapsed;
                        Si.Focus();
                        break;
                    case EnumButtonsMessage.Acept:
                        Si.Visibility = Visibility.Collapsed;
                        No.Visibility = Visibility.Collapsed;
                        Cancelar.Visibility = Visibility.Collapsed;
                        Aceptar.Focus();
                        break;
                    case EnumButtonsMessage.AceptCancel:
                        Si.Visibility = Visibility.Collapsed;
                        No.Visibility = Visibility.Collapsed;
                        Aceptar.Focus();
                        break;
                }

                switch (ButtonDefault)
                {
                    case EnumButtonsMessage.YesNo:
                        Si.Focus();
                        break;
                    case EnumButtonsMessage.YesNoCancel:
                        No.Focus();
                        break;
                    case EnumButtonsMessage.Acept:
                        Aceptar.Focus();
                        break;
                    case EnumButtonsMessage.AceptCancel:
                        Cancelar.Focus();
                        break;
                }

                if (MessageRequest.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length > 1)
                {
                    List<string> ListaLineas = new List<string>();
                    string TExtoEntero = string.Empty;

                    foreach (string line in MessageRequest.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                    {
                        ListaLineas.Add(line);
                    }

                    foreach (string Linea in ListaLineas)
                    {
                        Message += $"{Linea}{Environment.NewLine}";
                    }
                }
                else
                {
                    Message = MessageRequest;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void Si_Click(object sender, RoutedEventArgs e)
        {
            MessageResult = EnumMessageResult.Yes;
            Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            MessageResult = EnumMessageResult.No;
            Close();
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            MessageResult = EnumMessageResult.Acept;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageResult = EnumMessageResult.Cancel;
            Close();
        }

        #endregion

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
