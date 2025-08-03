using System;
using System.CodeDom.Compiler;
using System.Windows.Input;

namespace KCRV_Statistics.UI.AppService
{
    /// <summary>
    /// Содержит основную логику работы при использовании кнопок управления программой.
    /// </summary>
    public class Command : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        /// <summary>
        /// Идентификация экземпляра класса Command
        /// </summary>
        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Подписка на события
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}