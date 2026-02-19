using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rditil.Models;
using Rditil.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.ViewModels
{
    public enum EmailSendState
    {
        Idle,
        Sending,
        Sent,
        Failed
    }

    public partial class EndPageViewModel : ObservableObject, INavigable
    {
        private readonly IAppState _appState;
        private readonly IEmailService _emailService;

        // Résumé
        [ObservableProperty] private string userFullName = "";
        [ObservableProperty] private string userEmail = "";
        [ObservableProperty] private string managerEmail = "";
        [ObservableProperty] private string durationText = "00:00:00";
        [ObservableProperty] private string scoreText = "0/0";

        // Aperçu mail
        [ObservableProperty] private string mailBody = "";

        // Statut
        [ObservableProperty] private string statusText = "";

        // État d'envoi 
        [ObservableProperty] private EmailSendState emailState = EmailSendState.Idle;

        // Popup + fermeture
        [ObservableProperty] private bool showConfirmationPopup;
        [ObservableProperty] private int closeCountdown = 15;

        public ObservableCollection<ExamReportRow> ReportRows { get; } = new();

        public IAsyncRelayCommand SendAndCloseCommand { get; }

        private bool _autoSendTriggered;

        public EndPageViewModel(IAppState appState, IEmailService emailService)
        {
            _appState = appState;
            _emailService = emailService;

            SendAndCloseCommand = new AsyncRelayCommand(SendAndCloseAsync, () => IsEmailButtonEnabled);

            LoadFromState();
        }

        public void OnNavigatedTo(System.Collections.Generic.Dictionary<string, object?>? parameters)
        {
            LoadFromState();

            if (!_autoSendTriggered)
            {
                _autoSendTriggered = true;
                _ = SendAndCloseAsync();
            }
        }

        private void LoadFromState()
        {
            var r = _appState.LastExamResult;
            var rows = _appState.LastExamRows;

            UserFullName = r?.UserFullName ?? "";
            UserEmail = r?.UserEmail ?? "";

            ManagerEmail = _appState.ManagerEmail
                           ?? r?.ManagerEmail
                           ?? "";

            DurationText = (r?.Duration ?? TimeSpan.Zero).ToString(@"hh\:mm\:ss");
            ScoreText = r == null ? "0/0" : $"{r.Score}/{r.Total}";

            ReportRows.Clear();
            if (rows != null)
            {
                foreach (var row in rows)
                    ReportRows.Add(row);
            }

            MailBody = BuildPlainPreviewBody();
            StatusText = "";
            EmailState = EmailSendState.Idle;
            ShowConfirmationPopup = false;
            CloseCountdown = 15;

            SendAndCloseCommand.NotifyCanExecuteChanged();
        }

        private string BuildPlainPreviewBody()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Utilisateur : {UserFullName}");
            sb.AppendLine($"Email : {UserEmail}");
            sb.AppendLine($"Score : {ScoreText}");
            sb.AppendLine($"Durée : {DurationText}");
            sb.AppendLine();
            sb.AppendLine("Détails :");

            foreach (var r in ReportRows.Take(15))
            {
                var ok = r.IsCorrect ? "OK" : "FAUX";
                sb.AppendLine($"- {r.Index}. {Trim(r.Enonce, 70)} => {Trim(r.ChosenText, 45)} ({ok})");
            }

            if (ReportRows.Count > 15)
                sb.AppendLine($"... ({ReportRows.Count - 15} lignes de plus)");

            return sb.ToString();

            static string Trim(string s, int max)
                => string.IsNullOrWhiteSpace(s) ? "" : (s.Length <= max ? s : s.Substring(0, max) + "…");
        }

        // ===== Bouton dynamique =====
        public string EmailButtonText => EmailState switch
        {
            EmailSendState.Sending => "📨 Envoi en cours...",
            EmailSendState.Sent => "✅ Email envoyé",
            EmailSendState.Failed => "❌ Échec — réessayer",
            _ => "📩 Renvoyer le résultat"
        };

        public bool IsEmailButtonEnabled => EmailState != EmailSendState.Sending;

        partial void OnEmailStateChanged(EmailSendState value)
        {
            OnPropertyChanged(nameof(EmailButtonText));
            OnPropertyChanged(nameof(IsEmailButtonEnabled));
            SendAndCloseCommand.NotifyCanExecuteChanged();
        }

        private async Task SendAndCloseAsync()
        {
            try
            {
                EmailState = EmailSendState.Sending;
                StatusText = "Envoi en cours...";

                var to = ManagerEmail?.Trim();
                if (string.IsNullOrWhiteSpace(to))
                    throw new InvalidOperationException("Email du N+1 manquant.");

                var r = _appState.LastExamResult;
                if (r == null)
                    throw new InvalidOperationException("Aucun résultat d'examen.");

                var subject = $"RDITIL — Résultats {UserFullName} ({r.Score}/{r.Total})";

                await _emailService.SendExamReportAsync(
                    to: to,
                    cc: null,
                    subject: subject,
                    userFullName: UserFullName,
                    score: r.Score,
                    total: r.Total,
                    duration: r.Duration,
                    rows: ReportRows.ToList()
                );

                // OK
                EmailState = EmailSendState.Sent;
                StatusText = "Email envoyé avec succès ✅";

                // Affiche le popup + fermeture seulement APRÈS sent
                ShowConfirmationPopup = true;
                CloseCountdown = 15;

                while (CloseCountdown > 0)
                {
                    await Task.Delay(1000);
                    CloseCountdown--;
                }

                App.Current.Shutdown();
            }
            catch (Exception ex)
            {
                EmailState = EmailSendState.Failed;
                StatusText = $"Erreur : {ex.Message}";
            }
        }
    }
}
