using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rditil.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rditil.ViewModels
{
    public class ResultViewModel : ObservableObject
    {
        private readonly EmailService _emailService;

        private int _score;
        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        private int _totalQuestions;
        public int TotalQuestions
        {
            get => _totalQuestions;
            set => SetProperty(ref _totalQuestions, value);
        }

        private string _emailUtilisateur;
        public string EmailUtilisateur
        {
            get => _emailUtilisateur;
            set => SetProperty(ref _emailUtilisateur, value);
        }

        public double Pourcentage => TotalQuestions > 0
            ? (Score * 100.0 / TotalQuestions)
            : 0;

        public ICommand EnvoyerResultatParEmailCommand { get; }
        public ICommand QuitterCommand { get; }

        public ResultViewModel(
            EmailService emailService,
            string emailUtilisateur,
            int score,
            int totalQuestions)
        {
            _emailService = emailService;
            EmailUtilisateur = emailUtilisateur;
            Score = score;
            TotalQuestions = totalQuestions;

            EnvoyerResultatParEmailCommand = new AsyncRelayCommand(EnvoyerResultatParEmailAsync);
            QuitterCommand = new RelayCommand(QuitterApplication);
        }

        private async Task EnvoyerResultatParEmailAsync()
        {
            try
            {
                await _emailService.SendExamResultAsync(
                    EmailUtilisateur,
                    EmailUtilisateur,
                    Score,
                    TotalQuestions
                );

                MessageBox.Show(
                    "Résultat envoyé par email avec succès !",
                    "Envoi réussi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch
            {
                MessageBox.Show(
                    "Erreur lors de l'envoi du résultat par email.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void QuitterApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
