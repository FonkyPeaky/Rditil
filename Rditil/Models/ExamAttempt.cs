using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
{
    public class ExamAttempt
    {
        [Key]
        public int Id_ExamAttempt { get; set; }

        public int Id_Utilisateur { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt { get; set; }

        public int Score { get; set; }
        public int TotalQuestions { get; set; } = 40;

        // Navigation (si tu as Utilisateur)
        public Utilisateur? Utilisateur { get; set; }
    }
}
