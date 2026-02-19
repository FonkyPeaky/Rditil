using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rditil.Models
{
    public class Question
    {
        [Key]
        public int Id_Question { get; set; }

        public string? Enonce { get; set; }

        [NotMapped]
        public string? Intitule
        {
            get => Enonce;
            set => Enonce = value;
        }

        public ICollection<Reponse> Reponses { get; set; } = new List<Reponse>();
        public ICollection<Examen_Question> ExamenQuestions { get; set; } = new List<Examen_Question>();
    }
}
