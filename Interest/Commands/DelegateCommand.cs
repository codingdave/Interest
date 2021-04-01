using System;
using System.Windows.Input;

namespace Interest.Commands
{
    public class DelegateCommand : ICommand
    {
        Func<bool> _predicate;
        Action _execute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (value != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (value != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public DelegateCommand(Action execute, Func<bool> predicate = null)
        {
            _predicate = predicate;
            _execute = execute;
        }

        public bool CanExecute(object parameter = null)
        {
            var ret = true;
            if (_predicate != null)
            {
                ret = _predicate.Invoke();
            }
            return ret;
        }
        public void Execute(object parameter = null)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}