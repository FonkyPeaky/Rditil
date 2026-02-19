using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Rditil.Models;
using Rditil.Services;

namespace Rditil.ViewModels
{
    public class EndPageViewModel : ViewModelBase
    {
        private readonly IEmailService _emailService;

        public EndPageViewModel(IEmailService emailService)
        {
            _emailService = emailService;

            // Exemple valeurs (à remplacer par tes vraies infos)
            UserFullName = "Test User";
            ManagerEmail = "manager@company.com";

            Score = 6;
            Total = 40;

            // ✅ Evite "Duration" : on renomme
            ExamDuration = TimeSpan.FromMinutes(60);

            // ✅ Liste utilisée par l’email
            ReportRows = new ObservableCollection<ExamReportRow>();
        }

        public string UserFullName { get; set; } = "";
        public string ManagerEmail { get; set; } = "";

        public int Score { get; set; }
        public int Total { get; set; }

        // ✅ Renommé pour éviter le conflit
        public TimeSpan ExamDuration { get; set; }

        // ✅ Cette propriété manquait
        public ObservableCollection<ExamReportRow> ReportRows { get; set; }

        public bool IsSending { get; set; }
        public string StatusText { get; set; } = "";

        // Appelle ça quand tu arrives sur EndPage (ou quand tu finis l’exam)
        public void LoadReportRowsFromExam(/* ton objet résultat ici */)
        {
            // TODO: remplace par ta vraie source (questions/choix/utilisateur)
            // Exemple fake :
            ReportRows.Clear();
            ReportRows.Add(new ExamReportRow
            {
                Enonce = "Exemple question ?",
                ChosenText = "Réponse choisie",
                IsCorrect = false,
                CorrectText = "Bonne réponse"
            });
        }

        public async Task SendReportAsync()
        {
            try
            {
                IsSending = true;
                StatusText = "Envoi...";

                var subject = $"RDITIL - Résultats {UserFullName} ({Score}/{Total})";

                await _emailService.SendExamReportAsync(
                    to: ManagerEmail,
                    cc: null,
                    subject: subject,
                    userFullName: UserFullName,
                    score: Score,
                    total: Total,
                    duration: ExamDuration,
                    rows: ReportRows.ToList()
                );

                StatusText = "Email envoyé ✅";
            }
            catch (Exception ex)
            {
                StatusText = $"Erreur envoi: {ex.Message}";
            }
            finally
            {
                IsSending = false;
            }
        }
    }
}
