using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;

namespace Rditil.Services;

public class DbExamResultService
{
    private readonly AppDbContext _context;

    public DbExamResultService(AppDbContext context)
    {
        _context = context;
    }

    public void EnregistrerExamen(Utilisateur utilisateur, int score, List<Question> questions)
    {
        if (utilisateur == null) throw new ArgumentNullException(nameof(utilisateur));
        if (questions == null) throw new ArgumentNullException(nameof(questions));

        var now = DateTime.UtcNow;

        var attempt = new ExamAttempt
        {
            Id_Utilisateur = utilisateur.Id_Utilisateur,
            StartedAt = now,
            FinishedAt = now,
            Score = score,
            TotalQuestions = questions.Count
        };

        _context.ExamAttempts.Add(attempt);
        _context.SaveChanges();

        foreach (var q in questions)
        {
            _context.ExamAnswers.Add(new ExamAnswer
            {
                ExamAttemptId = attempt.Id_ExamAttempt,
                QuestionId = q.Id,
                SelectedReponseId = null,
                IsCorrect = false
            });
        }

        _context.SaveChanges();
    }
    public async Task<(ExamResultSummary Summary, List<ExamReportRow> Rows)> BuildReportAsync(int attemptId)
    {
        var attempt = await _context.ExamAttempts
            .Include(a => a.Utilisateur)
            .Include(a => a.Answers)
            .FirstOrDefaultAsync(a => a.Id_ExamAttempt == attemptId);

        if (attempt == null)
            throw new InvalidOperationException($"Attempt {attemptId} introuvable");

        var user = attempt.Utilisateur;
        var fullName = user == null ? "" : $"{user.Prenom} {user.Nom}".Trim();

        var rows = new List<ExamReportRow>();

        var questionIds = attempt.Answers.Select(a => a.QuestionId).Distinct().ToList();
        var questions = await _context.Questions
            .Where(q => questionIds.Contains(q.Id))
            .Include(q => q.Reponses)
            .ToDictionaryAsync(q => q.Id);

        foreach (var a in attempt.Answers)
        {
            if (!questions.TryGetValue(a.QuestionId, out var q))
                continue;

            var chosen = a.SelectedReponseId.HasValue
                ? q.Reponses.FirstOrDefault(r => r.Id_Reponse == a.SelectedReponseId.Value)
                : null;

            var correct = q.Reponses.FirstOrDefault(r => r.EstCorrect);

            rows.Add(new ExamReportRow
            {
                Enonce = string.IsNullOrWhiteSpace(q.Intitule) ? q.Enonce : q.Intitule,
                ChosenText = chosen?.TextReponse ?? "(aucune)",
                CorrectText = correct?.TextReponse ?? "(inconnue)",
                IsCorrect = a.IsCorrect
            });
        }

        var summary = new ExamResultSummary
        {
            UserFullName = fullName,
            UserEmail = user?.Email ?? string.Empty,
            ManagerEmail = user?.EmailNPlus1 ?? string.Empty,
            Score = attempt.Score,
            Total = attempt.Total,
            StartedAt = attempt.StartedAt,
            FinishedAt = (DateTime)attempt.FinishedAt,
        };

        return (summary, rows);
    }
}
