using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rditil.Common
{
    public sealed class RelayCommand : ICommand
    {
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
    }
}
