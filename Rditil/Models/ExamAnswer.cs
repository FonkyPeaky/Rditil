using Rditil.Data;
using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
{
    public class ExamAnswer
    {
        [Key]
        public int Id { get; set; }

        public int ExamAttemptId { get; set; }
        public ExamAttempt? ExamAttempt { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int? SelectedReponseId { get; set; }
        public Reponse SelectedReponse { get; set; }

        public bool IsCorrect { get; set; }
    }
}
