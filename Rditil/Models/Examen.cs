using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
{
    public class Examen
    {
        public ICollection<ExamAttempt> Attempts { get; set; } = [];


        [Key]
        public int Id_Examen { get; set; }

        public DateTime DateExamen { get; set; }

        public TimeSpan DureeExamen { get; set; }

        public int Score { get; set; }

        public int Id_Utilisateur { get; set; }
        public Utilisateur Utilisateur { get; set; } = null!;

        public ICollection<Examen_Question> ExamenQuestions { get; set; } = [];
    }
}
