﻿using System;
using System.Windows.Input;

namespace KCRV_Statistics.UI.AppService
{
    /// <summary>
    /// Содержит основную логику работы при использовании кнопок управления программой.
    /// </summary>
    public class Command : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Command(Action<object> _execute, Func<object, bool> _canExecute = null)
        {
            execute = _execute;
            canExecute = _canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}