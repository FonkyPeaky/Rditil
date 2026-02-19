using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rditil.Models
{
    [Table("exam_attempts", Schema = "public")]
    public class ExamAttempt
    {

        [Column("ExamenId")]
        public int ExamenId { get; set; }

        [Column("ExamenId_Examen")]
        public int ExamenId_Examen { get; set; }

        [ForeignKey(nameof(ExamenId_Examen))]
        public Examen? Examen { get; set; }

        public ICollection<ExamAnswer> Answers { get; set; } = new List<ExamAnswer>();

        [Key]
        [Column("id_exam_attempt")]
        public int Id_ExamAttempt { get; set; }

        [Column("id_utilisateur")]
        public int Id_Utilisateur { get; set; }

        [Column("StartedAt")]
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        [Column("FinishedAt")]
        public DateTime? FinishedAt { get; set; }

        [Column("Score")]
        public int Score { get; set; }

        [Column("TotalQuestions")]
        public int TotalQuestions { get; set; } = 40;

        [Column("UserFullName")]
        public string? UserFullName { get; set; }

        public Utilisateur? Utilisateur { get; set; }

        [NotMapped]
        public int Total => TotalQuestions;
    }
}
