<<<<<<< HEAD
using System;
using System.Windows.Input;
=======
﻿using System.Windows.Input;
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8

namespace Rditil;

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
<<<<<<< HEAD
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
=======
        private readonly Func<object?, bool> _canExecute;
        private readonly Func<object?, Task> _executeAsync;

        // CTOR pour async
        public RelayCommand(Func<object?, Task> executeAsync, Func<object?, bool>? canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute ?? (_ => true);
        }
        
        // CTOR pour sync
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
            : this(p =>
            {
                execute?.Invoke(p);
                return Task.CompletedTask;
            }, canExecute)
        { }
        
        public bool CanExecute(object? parameter) => _canExecute(parameter);
        public event EventHandler? CanExecuteChanged;
        
        public async void Execute(object? parameter) => await _executeAsync(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8
    }

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        if (execute == null) throw new ArgumentNullException(nameof(execute));
        _execute = _ => execute();
        if (canExecute != null)
            _canExecute = _ => canExecute();
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => _execute(parameter);

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    public void NotifyCanExecuteChanged() => RaiseCanExecuteChanged();
}
