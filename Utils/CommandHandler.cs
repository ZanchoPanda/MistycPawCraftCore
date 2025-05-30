using System;
using System.Windows.Input;

namespace MistycPawCraftCore.Utils
{
    public class CommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _handler;
        private bool _isEnabled;

        public RelayCommand(Action<object> handler)
        {
            _handler = handler;
            _isEnabled = true;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _handler(parameter);
        }
    }
}
