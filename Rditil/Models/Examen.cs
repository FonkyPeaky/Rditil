using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
{
    public class Examen
    {
        [Key]
        public int Id_Examen { get; set; }
        public ICollection<Examen_Question> ExamenQuestions { get; set; }

        public DateTime DateExamen { get; set; }
        public TimeSpan DureeExamen { get; set; }
        public int SCORE { get; set; }

        // Navigation
        public int Id_Utilisateur { get; set; }
        public Utilisateur Utilisateur { get; set; }

        public ICollection<Examen_Question> Examen_Questions { get; set; }
    }
}
