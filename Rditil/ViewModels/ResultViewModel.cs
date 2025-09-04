using System.Threading.Tasks;
using System.Windows.Input;
using Rditil.Common; // <-- RelayCommand
using Rditil.Services;

namespace Rditil.ViewModels
{
    public class ResultViewModel
    {
        private readonly IEmailService _emailService;

        public string EmailUtilisateur { get; }
        public int Score { get; }
        public int TotalQuestions { get; }

        public ICommand EnvoyerResultatParEmailCommand { get; }
        public ICommand QuitterCommand { get; }

        public ResultViewModel(IEmailService emailService,
                               string emailUtilisateur,
                               int score,
                               int totalQuestions)
        {
            _emailService = emailService;
            EmailUtilisateur = emailUtilisateur;
            Score = score;
            TotalQuestions = totalQuestions;

            // Async
            EnvoyerResultatParEmailCommand = new RelayCommand(async _ => await EnvoyerResultatParEmailAsync());

            // Sync (fermer l’appli, etc.)
            QuitterCommand = new RelayCommand(_ =>
            {
                QuitterApplication();
                return Task.CompletedTask;
            });
        }

        private async Task EnvoyerResultatParEmailAsync()
        {
            // adapte: to = Email du N+1 / cc = candidat
            await _emailService.SendExamResultAsync(
                to: "manager@exemple.local",
                cc: EmailUtilisateur,
                score: Score,
                total: TotalQuestions
            );
        }

        private void QuitterApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
