using System;
using System.Windows.Input;

namespace ToDoList.Commands
{
    public class RealyCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        private RealyCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute= execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RealyCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = _ => execute();
            if (canExecute != null)
            {
                _canExecute = _ => canExecute();
            }
            else
            {
                _canExecute = null;
            }
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged() 
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
