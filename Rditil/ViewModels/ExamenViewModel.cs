using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Rditil.Services;

namespace Rditil.ViewModels
{
    public class ExamenViewModel
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly INavigationService _navigationService;

        public int CurrentUserId { get; }
        private int _score;
        private int _total;

        public ICommand FinishExamCommand { get; }

        public ExamenViewModel(
            IUserService userService,
            IEmailService emailService,
            INavigationService navigationService,
            int currentUserId /* injecte ou set selon ta logique */)
        {
            _userService = userService;
            _emailService = emailService;
            _navigationService = navigationService;
            CurrentUserId = currentUserId;

            FinishExamCommand = new RelayCommand(async _ => await FinishExamAsync(), _ => true);
        }

        // Appelle ceci quand l’examen est terminé
        private async Task FinishExamAsync()
        {
            // calcule _score et _total selon ta logique
            var utilisateur = await _userService.GetByIdAsync(CurrentUserId);
            if (utilisateur is null)
            {
                // TODO: notifier l’UI
                return;
            }

            var to = !string.IsNullOrWhiteSpace(utilisateur.EmailNPlus1)
                ? utilisateur.EmailNPlus1!
                : "admin@exemple.local"; // fallback

            try
            {
                await _emailService.SendExamResultAsync(
                    to: to,
                    cc: utilisateur.Email,
                    score: _score,
                    total: _total
                );
            }
            catch (Exception ex)
            {
                // TODO: notifier l’UI (ex.Message) via un Dialog/Toast service
            }

            _navigationService.NavigateTo<ResultViewModel>();
        }
    }

    // RelayCommand simple (si tu n’en as pas)
    public sealed class RelayCommand : ICommand
    {
        private readonly Func<object?, bool> _canExecute;
        private readonly Func<object?, Task> _executeAsync;

        public RelayCommand(Func<object?, Task> executeAsync, Func<object?, bool>? canExecute = null)
        {
            _executeAsync = executeAsync;
            _canExecute = canExecute ?? (_ => true);
        }

        public bool CanExecute(object? parameter) => _canExecute(parameter);
        public event EventHandler? CanExecuteChanged;
        public async void Execute(object? parameter) => await _executeAsync(parameter);
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
