using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Models
{
    public class Question
    {
        [Key]
        public int Id_Question { get; set; }
        public string? Enonce { get; set; }

        public ICollection<Reponse>? Reponses { get; set; }
        public ICollection<Examen_Question>? ExamenQuestions { get; set; }
    }

}
